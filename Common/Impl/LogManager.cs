using CloudDaemon.Common.Interfaces;
using System;

namespace CloudDaemon.Common.Impl
{
    public static class LogManager
    {
        private static ILogger _logger;
        public static ILogger Logger {
            private get
            {
                if (_logger == null)
                {
                    _logger = new ConsoleLogger(); // default log : console
                }
                return _logger;
            }
            set
            {
                _logger = value;
            }
        }

        public static void Log(string message)
        {
            Logger.Log(message);
        }

        public static void Log(Exception ex)
        {
            Logger.Log(ex);
        }
    }
}
