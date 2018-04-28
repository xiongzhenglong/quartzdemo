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




            //var ceo_hpyJob =
            //   JobBuilder.Create<hpyJob>()
            //   .Build();
            //var ceo_mtJob =
            // JobBuilder.Create<mtJob>()
            // .Build();

            var ceo_bitJob =
             JobBuilder.Create<bitJob>()
             .Build();
            //  var ceo_eosJob =
            //JobBuilder.Create<eosJob>()
            //.Build();

            //     var ceo_oiocJob =
            //JobBuilder.Create<oiocJob>()
            //.Build();

            //var ceo_eacJob =
            //   JobBuilder.Create<eacJob>()
            //   .Build();

            // Schedule them to run


            //await scheduler.ScheduleJob(ceo_mtJob, CreateTrigger());
            await scheduler.ScheduleJob(ceo_bitJob, CreateTrigger());
            //await scheduler.ScheduleJob(ceo_oiocJob, CreateTrigger());
            //await scheduler.ScheduleJob(ceo_hpyJob, CreateTrigger());

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
