using ApplicationBackuper.Commands;
using ApplicationBackuper.Common;
using ApplicationBackuper.Configuration;
using System;
using System.Threading.Tasks;

namespace ApplicationBackuper.Components
{
    public class ServiceBackupComponent : IServiceBackupComponent
    {
        private readonly ICommandsExecutor _commandExecutor;
        private readonly IBackupComponent _archiver;
        private readonly AppConfiguration _configuration;
        private readonly ILogger _logger;

        public ServiceBackupComponent(
            ICommandsExecutor commandExecutor,
            IBackupComponent archiver,
            AppConfiguration configuration,
            ILogger logger)
        {
            _commandExecutor = commandExecutor ?? throw new ArgumentNullException(nameof(commandExecutor));
            _archiver = archiver ?? throw new ArgumentNullException(nameof(archiver));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Backup()
        {
            _logger.Log("Executing stop service commands");
            var exitCode = await _commandExecutor.Execute(_configuration.StopCommands);
            ThrowIfNotSuccess(exitCode);

            _logger.Log("Creating archive");
            await _archiver.Backup();

            _logger.Log("Executing start service commands");
            await _commandExecutor.Execute(_configuration.StartCommands);
            ThrowIfNotSuccess(exitCode);
        }

        private void ThrowIfNotSuccess(int exitCode)
        {
            if (exitCode == 0)
                return;

            var logCommand = $"Command exited with error exit code {exitCode}";
            _logger.Log(logCommand);
            throw new InvalidOperationException(logCommand);
        }
    }
}
