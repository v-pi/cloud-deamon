using CloudDaemon.Common.Interfaces;
using System;

namespace CloudDaemon.Common.Impl
{
    public class ConsoleLogger : ILogger
    {
        public void Log(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
