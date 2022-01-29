using ApplicationBackuper.Common;
using ApplicationBackuper.Configuration;
using System;
using System.IO;
using System.IO.Compression;
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
            var tempDirectory = Path.Combine(
                _configuration.Backup.OutputFolder,
                Path.GetFileNameWithoutExtension(archiveName));

            Directory.CreateDirectory(tempDirectory);

            foreach (var path in _configuration.Backup.Pathes)
            {
                CopyAll(path, tempDirectory);
            }

            _logger.Log($"Creating zip file {archiveName}...");
            ZipFile.CreateFromDirectory(tempDirectory, archiveName, CompressionLevel.Optimal, false);
            _logger.Log($"Zip file {archiveName} was created successfully");

            Directory.Delete(tempDirectory, true);

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

        private void CopyAll(string path, string targetFolder)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                _logger.Log($"File or directory does not exists {path}. Ignoring it.");
                return;
            }

            var attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                var sourceDirectory = new DirectoryInfo(path).Name;
                CopyDirectory(path, Path.Combine(targetFolder, sourceDirectory));
                return;
            }

            CopyFile(path, targetFolder);
        }

        private void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyDirectory(diSource, diTarget);
        }

        private void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (var file in source.GetFiles())
            {
                CopyFile(file, target);
            }

            foreach (var subDirectory in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(subDirectory.Name);
                CopyDirectory(subDirectory, nextTargetSubDir);
            }
        }

        private void CopyFile(string filePath, string directoryPath)
        {
            var file = new FileInfo(filePath);
            var directory = new DirectoryInfo(directoryPath);

            CopyFile(file, directory);
        }

        private void CopyFile(FileInfo file, DirectoryInfo target)
        {
            _logger.Log(@"Copying {0}\{1}", target.FullName, file.Name);
            file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }
    }
}
