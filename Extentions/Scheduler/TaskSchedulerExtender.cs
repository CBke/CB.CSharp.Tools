using Quartz;

namespace CB.CSharp.Extentions
{
    public static class TaskSchedulerExtender
    {
        public static void ScheduleDailyAt<T>(this IScheduler scheduler, string Identity, TimeOfDay TimeOfDay) where T : IJob
        {
            var JobDetail = JobBuilder.Create<T>()
            .WithIdentity(Identity)
            .Build();

            var Trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay)
                    .WithMisfireHandlingInstructionIgnoreMisfires()
                  )
                .ForJob(Identity)
                .Build();

            scheduler.ScheduleJob(JobDetail, Trigger);
        }
    }
}