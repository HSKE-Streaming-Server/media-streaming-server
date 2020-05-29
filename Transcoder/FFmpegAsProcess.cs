using Microsoft.Extensions.Configuration; //for IConfiguration
using Newtonsoft.Json; //for JsonConvert
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics; //for Process
using System.IO;
using System.Linq; //for StreamReader
using System.Reflection.PortableExecutable;
using System.Threading; //for Thread
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; //for Task

namespace Transcoder
{
    public class FFmpegAsProcess : ITranscoder
    {
        //TODO - Für transcodierte Videos Datenbankeinträge setzen (Referenz)
        //TODO - Videos auf dem Server löschen (nach 3 Tagen) sonst läufts voll
        //TODO - presets übergeben
        private readonly ILogger<FFmpegAsProcess> _logger;
        private readonly Dictionary<int, AudioPreset> _audioPresets = new Dictionary<int, AudioPreset>();
        private readonly Dictionary<int, VideoPreset> _videoPresets = new Dictionary<int, VideoPreset>();
        private readonly IConfiguration _config;
        private readonly string _webroot;

        public FFmpegAsProcess(ILogger<FFmpegAsProcess> logger)
        {
            _logger = logger;
            TranscoderCache = new List<TranscoderCachingObject>();

            //Read config file
            _config = new ConfigurationBuilder().AddJsonFile("TranscoderConfig.json", false, false).Build();
            _webroot = Path.Combine(_config["ApacheWebroot"]);
            //Presets

            var audioPresets = _config.GetChildren().First(item => item.Key == "AudioPresets").GetChildren();
            var videoPresets = _config.GetChildren().First(item => item.Key == "VideoPresets").GetChildren();

            //parse the audio presets from the config file
            foreach (var preset in audioPresets)
            {
                var values = preset.GetChildren().ToDictionary(item => item.Key, item => item.Value);
                _audioPresets.Add(int.Parse(values["Id"]), new AudioPreset(int.Parse(values["Id"]), values["Name"],
                    values["Description"], int.Parse(values["Bitrate"]), values["TranscoderArguments"]));
            }

            foreach (var preset in videoPresets)
            {
                var values = preset.GetChildren().ToDictionary(item => item.Key, item => item.Value);
                _videoPresets.Add(int.Parse(values["Id"]), new VideoPreset(int.Parse(values["Id"]), values["Name"],
                    values["Description"], int.Parse(values["ResolutionX"]), int.Parse(values["ResolutionY"]), int.Parse(values["Bitrate"]), values["TranscoderArguments"]));
            }
            _logger.LogInformation($"Transcoder found VideoPresets: {_videoPresets.Count()} AudioPresets: {_audioPresets.Count()}");

            _logger.LogInformation($"{nameof(FFmpegAsProcess)} initialized");

        }
        public IList<TranscoderCachingObject> TranscoderCache { get; }
        public Uri StartProcess(Uri uri, int videoPreset, int audioPreset)
        {
            _logger.LogInformation($"Starting transcode: Uri={uri},VideoPreset={videoPreset},AudioPreset={audioPreset}");
            if (!_videoPresets.ContainsKey(videoPreset))
            {
                _logger.LogTrace($"Throwing argument exception because {nameof(videoPreset)} is not contained in {nameof(_videoPresets)}");
                throw new ArgumentException("The specified video preset doesn't exist.");
            }

            if (!_audioPresets.ContainsKey(audioPreset))
            {
                _logger.LogTrace($"Throwing argument exception because {nameof(videoPreset)} is not contained in {nameof(_videoPresets)}");
                throw new ArgumentException("The specified audio preset doesn't exist.");
            }



            if (uri == null || string.IsNullOrWhiteSpace(uri.ToString()))
            {
                _logger.LogTrace($"Throwing argument exception because {nameof(uri)} is null, empty or whitespace");
                throw new ArgumentNullException(nameof(uri), "Uri cannot be null or empty.");
            }

            var timestamp = DateTime.Now;
            //2020-05-05-12:56:32
            var folderName = Path.Combine("transcoded", timestamp.ToString("yyyy-MM-dd-HH-mm-ss"));
            var folderPath = Path.Combine(_webroot, folderName);
            _logger.LogTrace($"Created folder {folderPath}");
            var selectedVideoPreset = _videoPresets[videoPreset];
            var selectedAudioPreset = _audioPresets[audioPreset];

            //cancellationTokenSource for Keepalive and Caching
            var cancellationTokenSource = new CancellationTokenSource();

            //.m3u8 will be passed and the transcoded files will be stored on the server -> C:/xampp/htdocs/ouput_mpd
            var parameter =
                $"-i \"{uri}\" {selectedVideoPreset.TranscoderArguments} {selectedAudioPreset.TranscoderArguments} -f dash {folderPath}/out.mpd";
            try
            {

                ProcessFFmpeg(parameter, folderPath, cancellationTokenSource);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Failed to start FFmpeg process with parameters: {parameter} in {folderPath}");
                throw;
            }
            //ProcessFFmpeg("ffmpeg -i \"https://zdf-hls-01.akamaized.net/hls/live/2002460/de/6225f4cab378772631347dd27372ea68/5/5.m3u8\" -c:a aac -strict experimental -c:v libx264 -s 240x320 -aspect 16:9 -f hls -hls_list_size 1000000 -hls_time 2 \"output/240_out.m3u8\"");


            //APIManager gets the 240_out.m3u8 ->im xampp /htdocs/output_mpd - Hardcoded
            var outUri = $"https://{_config["hostname"]}/{folderName}/out.mpd";
            _logger.LogDebug($"Gracefully returning {outUri} for request for {uri} with VideoPreset {videoPreset} and AudioPreset {audioPreset}");
            var transcodedVideoUri = new Uri(outUri);
            lock (TranscoderCache)
            {
                TranscoderCache.Add(new TranscoderCachingObject(uri, audioPreset, videoPreset, transcodedVideoUri, cancellationTokenSource));
            }
            return transcodedVideoUri;
        }

        public IEnumerable<VideoPreset> GetAvailableVideoPresets()
        {
            return _videoPresets.Select(item => item.Value);
        }

        public IEnumerable<AudioPreset> GetAvailableAudioPresets()
        {
            return _audioPresets.Select(item => item.Value);
        }


        /// <summary>
        /// Starts the FFmpeg as a process with the passed parameters.
        /// </summary>
        /// <param name="parameter">Declare arguments and options.</param>
        /// <param name="path">The path in which the transcoded fragments shall be located.</param>
        private Task ProcessFFmpeg(string parameter, string path, CancellationTokenSource cancellationTokenSource)
        {
            //TODO - Threads (Nuget Zeitstempel)

            Directory.CreateDirectory(path);
            Process ffmpegProcess = new Process
            {
                StartInfo =
                {
                    FileName = "ffmpeg",
                    Arguments = parameter,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = path
                }
            };
            _logger.LogTrace($"Created ffmpeg process with arguments: {parameter} in working directory: {path}");
            //With StandardError I get both normal output and errors (Because of "Error" in the name, it's probably confusing)
            //It's actually that what we see in the command prompt after we hit enter
            //Determined where the process is starting


            Task loggingTask = new Task(() =>
            {
                var logfilePath = Path.Combine(path, "transcoder.log");
                var logfile = new StreamWriter(File.OpenWrite(logfilePath));
                _logger.LogTrace($"Created transcoder logfile in {logfilePath}");
                StreamReader reader = ffmpegProcess.StandardError;

                logfile.WriteLine($"STARTING LOG AT {DateTime.Now:g}");
                logfile.WriteLine($"INVOKING FFMPEG WITH PARAMETERS: {parameter}");
                while (!reader.EndOfStream)
                {
                    try
                    {
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            logfile.WriteLine("KeepAlive Token invalidated, Process Suspended");
                            break;
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        logfile.WriteLine("KeepAlive Token invalidated, Process Suspended");
                        break;
                    }

                    while (reader.Peek() != -1)
                    {
                        var line = reader.ReadLine();
                        _logger.LogTrace($"PATH: {path}, LINE: {line}");
                        logfile.WriteLine(line);
                    }
                    logfile.Flush();
                    Thread.Sleep(250);

                    if (!ffmpegProcess.HasExited || !reader.EndOfStream) continue;
                    logfile.WriteLine("Process exited with code: " + ffmpegProcess.ExitCode);
                    _logger.LogInformation($"FFmpeg process in directory {path} exited with code {ffmpegProcess.ExitCode}");
                    ffmpegProcess.Dispose();
                    break;
                }
                logfile.Flush();
                logfile.Dispose();
                //Exception, if ffmpeg gets 404 response from input
                if (!ffmpegProcess.HasExited)
                    _logger.LogInformation($"Closing running FFmpeg process in {path}");
                ffmpegProcess.Close();
                try
                {
                    lock (TranscoderCache)
                    {
                        cancellationTokenSource.Cancel();
                    }
                }
                catch (ObjectDisposedException)
                {
                }
            });

            try
            {
                if (!ffmpegProcess.Start())
                {
                    throw new FFmpegProcessException(parameter, path, "Process.Start() returned false.");
                }
            }
            catch (Exception ex)
            {
                throw new FFmpegProcessException(ex, parameter, path, "Failed to start FFmpeg process");
            }

            loggingTask.Start();
            //Pause the thread until we see the file pop up
            var expectedFile = Path.Combine(path, "out.mpd");
            do
            {
                Thread.Sleep(50);
            } while (!File.Exists(expectedFile));
            _logger.LogTrace($"Expected file found at {expectedFile}");
            return loggingTask;
        }
    }
}