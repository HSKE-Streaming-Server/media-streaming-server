using Microsoft.Extensions.Configuration; //for IConfiguration
using System;
using System.Diagnostics; //for Process
using System.IO; //for StreamReader
using System.Reflection.PortableExecutable;
using System.Threading;

namespace Transcoder
{
    class FFmpegAsProcess : ITranscoder
    {
        //TODO - Für transcodierte Videos Datenbankeinträge setzen (Referenz)
        //TODO - Videos auf dem Server löschen (nach 3 Tagen) sonst läufts voll
        //TODO - presets übergeben
        private static FFmpegAsProcess _singleton = null;
        private IConfiguration Config { get; }
        private FFmpegAsProcess()
        {
            //Presets

            this.Config = new ConfigurationBuilder().AddJsonFile("TrancodedUriLocationConfig.json", false, true).Build();
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
        public string startProcess(Uri uri)
        {
            //.m3u8 will be passed and the transcoded files will be stored on the server -> C:/xampp/htdocs/ouput_mpd
            //ProcessFFmpeg("ffmpeg -i \"https://zdf-hls-01.akamaized.net/hls/live/2002460/de/6225f4cab378772631347dd27372ea68/5/5.m3u8\" -c:a aac -strict experimental -c:v libx264 -s 240x320 -aspect 16:9 -f hls -hls_list_size 1000000 -hls_time 2 \"output/240_out.m3u8\"");
            ProcessFFmpeg("ffmpeg -i \"" + uri + "\" -c:a aac -strict experimental -c:v libx264 -s 240x320 -aspect 16:9 -f hls -hls_list_size 1000000 -hls_time 2 \"output_mpd/240_out.m3u8\"");


            //TODO - IConfiguration; config -> Kontruktor (auslesen im startProcess)

            //APIManager gets the 240_out.m3u8 ->im xampp /htdocs/output_mpd - Hardcoded
            string uriForApiManager = Path.Combine(@"C:\xampp\", "htdocs", "output_mpd", "240_out.m3u8");
            return uriForApiManager;
            //return uri.ToString();
        }

        /// <summary>
        /// Starts the FFmpeg as a process with the passed parameters.
        /// </summary>
        /// <param name="parameter">Declare arguments and options.</param>
        private void ProcessFFmpeg(string parameter)
        {
            //TODO - Threads (Nuget Zeitstempel)
            Threader obj = new Threader();
            Thread th = new Thread(obj.doSomething);
            th.Start();
            Console.WriteLine("Press any key to kill threads!");
            Console.ReadKey();
            obj.kill();
            th.Join();
            Console.WriteLine();
            Console.ReadKey();

            Process proc = new Process();
            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = parameter;
            //With StandardError I get running status (Because of "Error" in the name, it's probably confusing)
            //It's actually that what we see in the command prompt after we hit enter
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            //Determined where the process is starting
            proc.StartInfo.WorkingDirectory = Path.Combine(@"C:\xampp\", "htdocs");

            try
            {
                if (!proc.Start()) { }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Process didn't started!\n\n" +
                                  ex.GetType() + ": " + ex.Message);
            }

            StreamReader reader = proc.StandardError;
            string OutputProgress = null;
            while ((OutputProgress = reader.ReadLine()) != null)
            {
                Console.WriteLine(OutputProgress);
            }
            proc.Close();
        }
        public class Threader
        {
            bool condition = false;
            public void doSomething()
            {
                while (!condition)
                {
                    Console.WriteLine("Thread is working!");
                    Thread.Sleep(1000);
                }
            }
            public void kill()
            {
                condition = true;
            }
        }
    }
}
