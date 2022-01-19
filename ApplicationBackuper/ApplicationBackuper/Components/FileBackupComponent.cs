using ApplicationBackuper.Common;
using ApplicationBackuper.Configuration;
using Aspose.Zip;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationBackuper.Components
{
    public class FileBackupComponent : IBackupComponent
    {
        private readonly AppConfiguration _configuration;
        private readonly ILogger _logger;

        public FileBackupComponent(AppConfiguration configuration, ILogger logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Backup()
        {
            var archiveName = CreateArchiveName();
            using var zipStream = File.Open(archiveName, FileMode.Create);
            using var archive = new Archive();

            foreach (var path in _configuration.Backup.Pathes)
            {
                _logger.Log($"Adding path {path} to archive...");
                AddEntry(archive, path);
            }

            archive.Save(zipStream);

            _logger.Log($"Zip file {archiveName} was created successfully");

            return Task.CompletedTask;
        }

        private string CreateArchiveName()
        {
            var archiveName = $"{_configuration.Name}_{DateTime.UtcNow:o}.zip";
            var correctArchiveName = ReplaceInvalidChars(archiveName);
            return $"{_configuration.Backup.OutputFolder}{Path.DirectorySeparatorChar}{correctArchiveName}";
        }

        private string ReplaceInvalidChars(string archiveName)
        {
            var invalidChars = Path.GetInvalidPathChars()
                .Union(new[] { ':' })
                .ToArray();
            return string.Join("_", archiveName.Split(invalidChars));
        }

        private void AddEntry(Archive archive, string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                _logger.Log($"File path {path} does not exists. Skipping it");
                return;
            }

            var attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                var directory = new DirectoryInfo(path);
                archive.CreateEntries(directory);
                return;
            }

            var file = new FileInfo(path);
            archive.CreateEntry(file.Name, file);
        }
    }
}
