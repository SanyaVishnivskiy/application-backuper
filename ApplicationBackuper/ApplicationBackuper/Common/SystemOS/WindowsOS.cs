namespace ApplicationBackuper.Common.SystemOS
{
    public class WindowsOS : IOS
    {
        private static IOSShell _shell = new WindowsCmdShell();

        public IOSShell Shell => _shell;
    }

    public class WindowsCmdShell : IOSShell
    {
        public string Path => "cmd.exe";

        public string ArgsPrefix => "/C ";
    }
}
