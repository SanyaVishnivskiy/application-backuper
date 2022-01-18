using ApplicationBackuper.Commands;
using ApplicationBackuper.Common;
using ApplicationBackuper.Configuration;
using System;
using System.Threading.Tasks;

namespace ApplicationBackuper.Components
{
    public class BackupComponent : IBackupComponent
    {
        private readonly ICommandsExecutor _commandExecutor;
        private readonly AppConfiguration _configuration;
        private readonly ILogger _logger;

        public BackupComponent(
            ICommandsExecutor commandExecutor,
            AppConfiguration configuration,
            ILogger logger)
        {
            _commandExecutor = commandExecutor ?? throw new ArgumentNullException(nameof(commandExecutor));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Backup()
        {
            _logger.Log("Executing stop service commands");
            await _commandExecutor.Execute(_configuration.StopCommands);

            // Read files for backup
            // Archive files
            // save to output folder

            _logger.Log("Executing start service commands");
            await _commandExecutor.Execute(_configuration.StartCommands);
        }
    }
}
