using ApplicationBackuper.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationBackuper.Commands
{
    public class BashCommandsExecutor : ICommandsExecutor
    {
        private readonly ILogger _logger;

        public BashCommandsExecutor(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Execute(List<string> commands)
        {
            var commandArgs = string.Join(" && ", commands);

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(OS.Shell.Path)
                {
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = OS.Shell.ArgsPrefix + commandArgs
                }
            };

            process.Start();
            await process.WaitForExitAsync();

            using (StreamReader sr = process.StandardOutput)
            {
                _logger.Debug("Output: " + sr.ReadToEnd());
            }

            using (StreamReader sr = process.StandardError)
            {
                _logger.Debug("Error: " + sr.ReadToEnd());
            }

            return process.ExitCode;
        }
    }
}
