using Microsoft.Extensions.Configuration; //for IConfiguration
using System;
using System.Collections.Generic;
using System.Diagnostics; //for Process
using System.IO; //for StreamReader
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace Transcoder
{
    public class FFmpegAsProcess : ITranscoder
    {
        //TODO - Für transcodierte Videos Datenbankeinträge setzen (Referenz)
        //TODO - Videos auf dem Server löschen (nach 3 Tagen) sonst läufts voll
        //TODO - presets übergeben
        private static FFmpegAsProcess _singleton;
        private IConfiguration _config;
        private string _webroot;
        private FFmpegAsProcess()
        {
            //Read config file
            _config = new ConfigurationBuilder().AddJsonFile("TranscoderConfig.json", false, false).Build();
            _webroot = Path.Combine(_config["ApacheWebroot"], "transcoded");
            //Presets

        }

        static FFmpegAsProcess()
        {
            _singleton = new FFmpegAsProcess();
        }

        public static FFmpegAsProcess GetSingleton()
        {
            return _singleton ??= new FFmpegAsProcess();
        }

        //TODO - besseren Namen vergeben (mit Felix abklären); -> Transcode 
        public string StartProcess(Uri uri, int videoPreset, int audioPreset)
        {
            var timestamp = DateTime.Now;
            //2020-05-05-12:56:32
            var folderName = timestamp.ToString("yyyy-MM-dd-HH:mm:ss");
            var folderPath = Path.Combine(_webroot, folderName);
            //.m3u8 will be passed and the transcoded files will be stored on the server -> C:/xampp/htdocs/ouput_mpd
            //ProcessFFmpeg("ffmpeg -i \"https://zdf-hls-01.akamaized.net/hls/live/2002460/de/6225f4cab378772631347dd27372ea68/5/5.m3u8\" -c:a aac -strict experimental -c:v libx264 -s 240x320 -aspect 16:9 -f hls -hls_list_size 1000000 -hls_time 2 \"output/240_out.m3u8\"");
            ProcessFFmpeg($"ffmpeg -i \"{uri}\" -c:a aac -strict experimental -c:v libx264 -s 240x320 -aspect 16:9 -f hls -hls_list_size 1000000 -hls_time 2 {folderPath}/240_out.m3u8", folderPath);


            //TODO - IConfiguration; config -> Kontruktor (auslesen im startProcess)

            //APIManager gets the 240_out.m3u8 ->im xampp /htdocs/output_mpd - Hardcoded
            string uriForApiManager = Path.Combine(folderPath, "240_out.m3u8");
            return uriForApiManager;
            //return uri.ToString();
        }

        public IEnumerable<VideoPreset> GetAvailableVideoPresets()
        {
            return new List<VideoPreset>
            {
                new VideoPreset(1, "test", "240x240", "128kbps", "gemachtelfing")
            };
        }

        public IEnumerable<AudioPreset> GetAvailableAudioPresets()
        {
            return new List<AudioPreset>
            {
                new AudioPreset(1, "test", "32kbit/s", "samuchtafeng")
            };
        }


        /// <summary>
        /// Starts the FFmpeg as a process with the passed parameters.
        /// </summary>
        /// <param name="parameter">Declare arguments and options.</param>
        /// <param name="path">The path in which the transcoded fragments shall be located.</param>
        private Task ProcessFFmpeg(string parameter, string path)
        {
            //TODO - Threads (Nuget Zeitstempel)

            Process proc = new Process
            {
                StartInfo =
                {
                    FileName = "ffmpeg",
                    Arguments = parameter,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.Combine(@"C:\xampp\", "htdocs")
                }
            };
            //With StandardError I get both normal output and errors (Because of "Error" in the name, it's probably confusing)
            //It's actually that what we see in the command prompt after we hit enter
            //Determined where the process is starting

            try
            {
                if (!proc.Start())
                {
                    throw new Exception("Process.Start() returned false.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Process didn't started!\n\n" +
                                  ex.GetType() + ": " + ex.Message);
            }
            
            Task task = new Task(() =>
            {
                var logfile = new StreamWriter(File.OpenWrite(path + "/transcoder.log"));
                StreamReader reader = proc.StandardError;
                
                while (!reader.EndOfStream)
                {
                    if(reader.Peek()!=-1)
                        logfile.WriteLine(reader.ReadLine());
                    Thread.Sleep(250);
                }
                proc.Close();
            });

            task.Start();
            return task;
        }
        
    }
}
