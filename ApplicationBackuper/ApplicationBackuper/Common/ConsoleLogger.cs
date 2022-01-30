using System;

namespace ApplicationBackuper.Common
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string message)
        {
            Debug(message, Array.Empty<string>());
        }

        public void Debug(string message, params string[] parameters)
        {
            Console.WriteLine("[DEBUG] [START]=============================");
            Console.WriteLine("[DEBUG] " + string.Format(message, parameters));
            Console.WriteLine("[DEBUG] [END]=============================");
        }

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
