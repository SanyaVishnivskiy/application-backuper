using ApplicationBackuper.Configuration;
using ApplicationBackuper.Jobs;
using FluentScheduler;
using Microsoft.Extensions.Configuration;
using System;

namespace ApplicationBackuper.Composition
{
    public class CompositionRoot
    {
        public void Initialize()
        {
            var configuration = InitializeConfiguration();

            InitJobs(configuration);
        }

        private void InitJobs(FullConfiguration configuration)
        {
            JobManager.Initialize();
            JobManager.AddJob<BackupJob>(s => {
                var schedule = s.ToRunNow();
                foreach (var time in configuration.Maintanance.RunEveryDayAt)
                {
                    if (TimeSpan.TryParse(time, out var timeSpan))
                    {
                        schedule
                            .AndEvery(0)
                            .Days()
                            .At(timeSpan.Hours, timeSpan.Minutes);
                    }
                }
            });
        }

        private FullConfiguration InitializeConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            var fullConfig = new FullConfiguration();
            configuration.Bind(fullConfig);

            return fullConfig;
        }
    }
}
