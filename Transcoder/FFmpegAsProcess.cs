using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Data.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;

namespace Transcoder
{
    public class FFmpegAsProcess : ITranscoder
    {
        private IConfiguration ConfigDB { get; }
        private string SqlConnectionString { get; }
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

            ConfigDB = new ConfigurationBuilder().AddJsonFile("./GrabberConfig.json", false, true).Build();
            SqlConnectionString = $"Server={ConfigDB["MySqlServerAddress"]};" +
                                  $"Database={ConfigDB["MySqlServerDatabase"]};" +
                                  $"Uid={ConfigDB["MySqlServerUser"]};" +
                                  $"Pwd={ConfigDB["MySqlServerPassword"]};";
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
                throw new ApiNotFoundException("The specified video preset doesn't exist.");
            }

            if (!_audioPresets.ContainsKey(audioPreset))
            {
                _logger.LogTrace($"Throwing argument exception because {nameof(videoPreset)} is not contained in {nameof(_videoPresets)}");
                throw new ApiNotFoundException("The specified audio preset doesn't exist.");
            }



            if (uri == null || string.IsNullOrWhiteSpace(uri.ToString()))
            {
                _logger.LogTrace($"Throwing argument exception because {nameof(uri)} is null, empty or whitespace");
                throw new ApiBadRequestException("Uri cannot be null or empty.");
            }

            //Check if MpdLink && VideoPreset && AudioPreset already exists, then return MpdPath Server.
            using (var dbConnection = new MySqlConnection(SqlConnectionString))
            {
                try
                {
                    var selectQuery = "SELECT * FROM mediacontent.alreadytranscodedmpd WHERE MpdLink=@MpdLink AND VideoPreset=@VideoPreset AND AudioPreset=@AudioPreset";
                    var selectCommand = new MySqlCommand(selectQuery, dbConnection);
                    {
                        selectCommand.Parameters.Add(new MySqlParameter("@MpdLink", uri));
                        selectCommand.Parameters.Add(new MySqlParameter("@VideoPreset", videoPreset));
                        selectCommand.Parameters.Add(new MySqlParameter("@AudioPreset", audioPreset));
                    }
                    //Read in the SELECT results
                    MySqlDataReader reader = selectCommand.ExecuteReader();
                    if (uri.ToString().Equals(reader.GetString("MpdLink")) &&
                        videoPreset.ToString().Equals(reader.GetString("VideoPreset")) &&
                        audioPreset.ToString().Equals(reader.GetString("AudioPreset")))
                    {
                        var outServerUri = reader.GetString("MpdPathServer");
                        var alreadyTranscodedVideoUri = new Uri(outServerUri);
                        return alreadyTranscodedVideoUri;
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                }
            }
            //If record does not exist in the database the directory will be created and ProcessFFmpeg will be called

            var timestamp = DateTime.Now;
            //2020-05-05-12:56:32
            var folderName = Path.Combine("transcoded", timestamp.ToString("yyyy-MM-dd-HH-mm-ss"));
            var folderPath = Path.Combine(_webroot, folderName);
            _logger.LogTrace($"Created folder {folderPath}");
            var selectedVideoPreset = _videoPresets[videoPreset];
            var selectedAudioPreset = _audioPresets[audioPreset];

            //cancellationTokenSource for Keepalive and Caching
            var cancellationTokenSource = new CancellationTokenSource();

            var parameter =
                $"-i \"{uri}\" {selectedVideoPreset.TranscoderArguments} {selectedAudioPreset.TranscoderArguments} -f dash {folderPath}/out.mpd";
            try
            {

                ProcessFFmpeg(parameter, folderPath, cancellationTokenSource);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Failed to start FFmpeg process with parameters: {parameter} in {folderPath}");
                throw new Exception("Internal FFmpeg Error");
            }

            var outUri = $"https://{_config["hostname"]}/{folderName}/out.mpd";
            _logger.LogDebug($"Gracefully returning {outUri} for request for {uri} with VideoPreset {videoPreset} and AudioPreset {audioPreset}");
            var transcodedVideoUri = new Uri(outUri);
            lock (TranscoderCache)
            {
                TranscoderCache.Add(new TranscoderCachingObject(uri, audioPreset, videoPreset, transcodedVideoUri, cancellationTokenSource));
            }

            //Already transcoded manifest will be moved from "transcoded" to "AlreadyTranscodedReuse" and reused
            var folderNameAlreadyTranscodedReuse = Path.Combine("AlreadyTranscodedReuse");
            var folderPathAlreadyTranscodedReuse = Path.Combine(_webroot, folderNameAlreadyTranscodedReuse);
            //If processing finished correctly only then move to "AlreadyTranscodedReuse"
            var folderPathExitCode = Path.Combine(folderPath, "transcoder.log");
            if (File.ReadAllText(folderPathExitCode).Contains("Process exited with code: 0"))
            {
                //Create all of the directories
                foreach (var newPath in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(folderNameAlreadyTranscodedReuse);
                //Copy all the files
                foreach (var newPath in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(folderPath, folderPathAlreadyTranscodedReuse, true);
                }

                using (var dbConnection = new MySqlConnection(SqlConnectionString))
                {
                    try
                    {
                        var insertQuery = "INSERT INTO mediacontent.alreadytranscodedmpd (MpdLink, VideoPreset, AudioPreset) VALUES (@MpdLink, @VideoPreset, @AudioPreset)";
                        var insertCommand = new MySqlCommand(insertQuery, dbConnection);

                        insertCommand.Parameters.AddWithValue("@MpdLink", uri);
                        insertCommand.Parameters.AddWithValue("@MpdPathServer", folderPathAlreadyTranscodedReuse);
                        insertCommand.Parameters.AddWithValue("@VideoPreset", videoPreset);
                        insertCommand.Parameters.AddWithValue("@AudioPreset", audioPreset);

                        if (insertCommand.ExecuteNonQuery() == 1)
                            _logger.LogDebug($"Data Inserted");
                        else
                            _logger.LogError($"Failed: Data Not Inserted");
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception.Message);
                    }
                }
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
        /// <param name="cancellationTokenSource"></param>
        private void ProcessFFmpeg(string parameter, string path, CancellationTokenSource cancellationTokenSource)
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
        }
    }
}