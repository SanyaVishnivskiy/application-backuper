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

        public async Task Execute(List<string> commands)
        {
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo(OS.Platform.BashPath)
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                foreach (var command in commands)
                {
                    await sw.WriteLineAsync(command);
                }

                p.StandardInput.WriteLine("exit");
            }

            //using (StreamReader sr = p.StandardOutput)
            //{
            //    _logger.Log(sr.ReadToEnd());
            //}

            //using (StreamReader sr = p.StandardError)
            //{
            //    _logger.Log(sr.ReadToEnd());
            //}
        }
    }
}
