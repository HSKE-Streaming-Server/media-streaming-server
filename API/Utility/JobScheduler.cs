using Quartz;
using Quartz.Impl;

namespace API.Utility
{
    public class JobScheduler
    {
        private static IScheduler _scheduler;
        public static void Start()
        {
            var factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler().Result;

            _scheduler.Start();

            IJobDetail job = JobBuilder.Create<DeleteFilesJob>().WithIdentity("deleteJob").Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("deleteTrigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        public static void Shutdown()
        {
            _scheduler.Shutdown();
        }
    }
}