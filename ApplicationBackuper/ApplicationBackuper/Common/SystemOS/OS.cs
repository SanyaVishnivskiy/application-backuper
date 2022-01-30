using ApplicationBackuper.Common.SystemOS;
using System;
using System.Runtime.InteropServices;

namespace ApplicationBackuper.Common
{
    public interface IOS
    {
        IOSShell Shell { get; }
    }

    public interface IOSShell
    {
        string Path { get; }
        string ArgsPrefix { get; }
    }

    public static class OS
    {
        private static IOS _os;

        static OS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _os = new LinuxOS();
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _os = new WindowsOS();
                return;
            }

            throw new Exception("OS not supported");
        }

        public static IOSShell Shell => _os.Shell;
    }
}
