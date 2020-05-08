using Microsoft.Extensions.Configuration; //for IConfiguration
using Newtonsoft.Json; //for JsonConvert
using System;
using System.Collections.Generic;
using System.Diagnostics; //for Process
using System.IO;
using System.Linq; //for StreamReader
using System.Reflection.PortableExecutable;
using System.Threading; //for Thread
using System.Threading.Tasks; //for Task

namespace Transcoder
{
    public class FFmpegAsProcess : ITranscoder
    {
        //TODO - Für transcodierte Videos Datenbankeinträge setzen (Referenz)
        //TODO - Videos auf dem Server löschen (nach 3 Tagen) sonst läufts voll
        //TODO - presets übergeben
        private static FFmpegAsProcess _singleton;


        private Dictionary<int, AudioPreset> _audioPresets = new Dictionary<int, AudioPreset>();
        private Dictionary<int, VideoPreset> _videoPresets = new Dictionary<int, VideoPreset>();
        private IConfiguration _config;
        private string _webroot;

        private FFmpegAsProcess()
        {
            //Read config file
            _config = new ConfigurationBuilder().AddJsonFile("TranscoderConfig.json", false, false).Build();
            _webroot = Path.Combine(_config["ApacheWebroot"], "transcoded");
            //Presets

            var audioPresets = _config.GetChildren().First(item => item.Key == "AudioPresets").GetChildren();
            var videoPresets = _config.GetChildren().First(item => item.Key == "VideoPresets").GetChildren();

            //parse the audio presets from the config file
            foreach (var preset in audioPresets)
            {
                var values = preset.GetChildren().ToDictionary(item => item.Key, item => item.Value);
                _audioPresets.Add(int.Parse(values["Id"]), new AudioPreset(int.Parse(values["Id"]), values["Name"],
                    values["Description"], values["Bitrate"], values["TranscoderArguments"]));
            }

            foreach (var preset in videoPresets)
            {
                var values = preset.GetChildren().ToDictionary(item => item.Key, item => item.Value);
                _videoPresets.Add(int.Parse(values["Id"]), new VideoPreset(int.Parse(values["Id"]), values["Name"],
                    values["Description"], values["Resolution"], values["Bitrate"], values["TranscoderArguments"]));
            }
        }

        static FFmpegAsProcess()
        {
            _singleton = new FFmpegAsProcess();
        }

        public static FFmpegAsProcess GetSingleton()
        {
            return _singleton ??= new FFmpegAsProcess();
        }

        public string StartProcess(Uri uri, int videoPreset, int audioPreset)
        {
            if (!_videoPresets.ContainsKey(videoPreset))
                throw new ArgumentException("The specified video preset doesn't exist.");
            if (!_audioPresets.ContainsKey(audioPreset))
                throw new ArgumentException("The specified audio preset doesn't exist.");
            if (uri == null || string.IsNullOrEmpty(uri.ToString()))
                throw new ArgumentNullException(nameof(uri), "Uri cannot be null or empty.");

            var timestamp = DateTime.Now;
            //2020-05-05-12:56:32
            var folderName = timestamp.ToString("yyyy-MM-dd-HH-mm-ss");
            var folderPath = Path.Combine(_webroot, folderName);
            var selectedVideoPreset = _videoPresets[videoPreset];
            var selectedAudioPreset = _audioPresets[audioPreset];

            //.m3u8 will be passed and the transcoded files will be stored on the server -> C:/xampp/htdocs/ouput_mpd

            ProcessFFmpeg(
                $"-i \"{uri}\" {selectedVideoPreset.TranscoderArguments} {selectedAudioPreset.TranscoderArguments} -f dash {folderPath}/out.mpd",
                folderPath);

            //ProcessFFmpeg("ffmpeg -i \"https://zdf-hls-01.akamaized.net/hls/live/2002460/de/6225f4cab378772631347dd27372ea68/5/5.m3u8\" -c:a aac -strict experimental -c:v libx264 -s 240x320 -aspect 16:9 -f hls -hls_list_size 1000000 -hls_time 2 \"output/240_out.m3u8\"");


            //APIManager gets the 240_out.m3u8 ->im xampp /htdocs/output_mpd - Hardcoded
            return Path.Combine(_webroot, folderName, "out.mpd");
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
        private Task ProcessFFmpeg(string parameter, string path)
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
            //With StandardError I get both normal output and errors (Because of "Error" in the name, it's probably confusing)
            //It's actually that what we see in the command prompt after we hit enter
            //Determined where the process is starting


            Task loggingTask = new Task(() =>
            {
                var logfile = new StreamWriter(File.OpenWrite(path + "/transcoder.log"));

                StreamReader reader = ffmpegProcess.StandardError;

                logfile.WriteLine($"STARTING LOG AT {DateTime.Now:g}");
                logfile.WriteLine($"INVOKING FFMPEG WITH PARAMETERS: {parameter}");
                while (!reader.EndOfStream)
                {
                    while (reader.Peek() != -1)
                        logfile.WriteLine(reader.ReadLine());
                    logfile.Flush();
                    Thread.Sleep(250);

                    if (!ffmpegProcess.HasExited||!reader.EndOfStream) continue;
                    logfile.WriteLine("Process exited with code: " + ffmpegProcess.ExitCode);
                    ffmpegProcess.Dispose();
                    break;
                }
                logfile.Flush();
                logfile.Dispose();
                ffmpegProcess.Close();
            });


            try
            {
                if (!ffmpegProcess.Start())
                {
                    throw new Exception("Process.Start() returned false.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Process didn't started!\n\n" +
                                  ex.GetType() + ": " + ex.Message);
            }

            loggingTask.Start();

            return loggingTask;
        }
    }
}