using System.Collections.Generic;

namespace ApplicationBackuper.Configuration
{
    public class AppConfiguration
    {
        public string Name { get; set; }
        public List<string> StartCommands { get; set; } = new();
        public List<string> StopCommands { get; set; } = new();
        public AppBackupConfiguration Backup { get; set; }
    }

    public class AppBackupConfiguration
    {
        public string OutputFolder { get; set; }
        public List<string> Pathes { get; set; } = new();
    }
}
