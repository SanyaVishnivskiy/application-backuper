namespace ApplicationBackuper.Common.SystemOS
{
    public class LinuxOS : IOS
    {
        private static IOSShell _shell = new LinuxBashShell();

        public IOSShell Shell => _shell;
    }

    public class LinuxBashShell : IOSShell
    {
        public string Path => "/bin/bash";

        public string ArgsPrefix => "";
    }
}
