using System;
using System.Threading.Tasks;
using Quartz;

namespace QuartzDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            var factory = new Quartz.Impl.StdSchedulerFactory();
            factory.Initialize();
            var scheduler = await factory.GetScheduler();

            // Create some jobs

           

            var ceoJob =
                JobBuilder.Create<CEOJob>()
                .Build();

            // Schedule them to run

            
            await scheduler.ScheduleJob(ceoJob, CreateTrigger());

            await scheduler.Start();

            Console.WriteLine("Scheduler started, press any key to quit");
            Console.ReadKey();

            // Stop running all jobs

            await scheduler.Shutdown();
        }

        static ITrigger CreateTrigger()
        {
            return TriggerBuilder.Create()
                  .WithSimpleSchedule(s => s
                      .WithIntervalInSeconds(5)
                      .RepeatForever())
                  .StartNow()
                  .Build();
        }
    }
}
