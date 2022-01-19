using ApplicationBackuper.Commands;
using ApplicationBackuper.Common;
using ApplicationBackuper.Components;
using ApplicationBackuper.Configuration;
using ApplicationBackuper.Jobs;
using FluentScheduler;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ApplicationBackuper.Composition
{
    public class CompositionRoot
    {
        private ConsoleLogger _logger = new();

        public void Initialize()
        {
            var configuration = InitializeConfiguration();

            InitJobs(configuration);
        }

        private void InitJobs(FullConfiguration configuration)
        {
            JobManager.Initialize();
            JobManager.AddJob(
                CreateBackupJob(configuration.App),
                s => {
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

        private BackupJob CreateBackupJob(AppConfiguration config)
        {
            if (!Directory.Exists(config.Backup.OutputFolder))
            {
                Directory.CreateDirectory(config.Backup.OutputFolder);
            }

            var commandExecutor = new BashCommandsExecutor(_logger);
            var backupComponent = new FileBackupComponent(config, _logger);

            var component = new ServiceBackupComponent(
                commandExecutor,
                backupComponent,
                config,
                _logger);

            return new BackupJob(component, _logger);
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
