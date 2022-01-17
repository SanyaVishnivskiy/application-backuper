using System;

namespace ApplicationBackuper.Common
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, params string[] parameters)
        {
            Console.WriteLine(string.Format(message, parameters));
        }

        public void Log(Exception e, string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(e);
        }
    }
}
