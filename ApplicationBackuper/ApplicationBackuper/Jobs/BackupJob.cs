using ApplicationBackuper.Common;
using ApplicationBackuper.Components;
using FluentScheduler;
using System;
using System.Threading.Tasks;

namespace ApplicationBackuper.Jobs
{
    public class BackupJob : IAsyncJob
    {
        private readonly IServiceBackupComponent _component;
        private readonly ILogger _logger;

        public BackupJob(
            IServiceBackupComponent component,
            ILogger logger)
        {
            _component = component ?? throw new ArgumentNullException(nameof(component));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.Log($"Starting job {nameof(BackupJob)}");

                await _component.Backup();

                _logger.Log($"{nameof(BackupJob)} has ended successfully");
            }
            catch (Exception e)
            {
                _logger.Log(e, $"An error occured while execution job {nameof(BackupJob)}");
            }
        }
    }
}
