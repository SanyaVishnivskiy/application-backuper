using ApplicationBackuper.Common;
using FluentScheduler;
using System.Threading.Tasks;

namespace ApplicationBackuper.Jobs
{
    public class BackupJob : IAsyncJob
    {
        public Task ExecuteAsync()
        {
            var logger = new ConsoleLogger();
            logger.Log($"Starting job {nameof(BackupJob)}");


            logger.Log($"{nameof(BackupJob)}Job has ended");
            return Task.CompletedTask;
        }
    }
}
