using System;

namespace ApplicationBackuper.Common
{
    public interface ILogger
    {
        void Log(string message);
        void Log(Exception e, string message);
        void Log(string message, params string[] parameters);
    }
}
