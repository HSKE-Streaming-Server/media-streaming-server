using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Setup NLog logger
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Initializing main");
                CreateHostBuilder(args).Build().Run();
            }
            //Log all exceptions that arise while starting the server
            catch (Exception ex)
            {
                logger.Error(ex, "Program stopped because of exception!");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        //Uncomment the following line if you want log messages to be output to the console aswell
                        logging.AddConsole();
                        //Set minimum log level to trace, so we can log absolutely everything
                        logging.SetMinimumLevel(LogLevel.Trace);
                    }).UseNLog(); //add NLog as a Logging provider
                });
    }
}
