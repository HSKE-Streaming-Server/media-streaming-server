using System;
using API.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Initializing main");
                
                JobScheduler.Start();
                var host = CreateHostBuilder(args).Build();

                var config = host.Services.GetRequiredService<IConfiguration>();

                foreach (var c in config.AsEnumerable())
                {
                    logger.Debug(c.Key + " = " + c.Value);
                }
                
                host.Run();
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
                JobScheduler.Shutdown();
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
