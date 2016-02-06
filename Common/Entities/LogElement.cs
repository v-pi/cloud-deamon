using CloudDaemon.Common.Enum;
using System;

namespace CloudDaemon.Common.Entities
{
    public class LogElement
    {
        public DateTime Time { get; private set; }

        public LogLevel LogLevel { get; private set; }

        public object Log { get; private set; }

        public LogElement(LogLevel logLevel, object log)
        {
            Time = DateTime.Now;
            LogLevel = logLevel;
            Log = log;
        }

        public override string ToString()
        {
            return String.Format("{0:O}\t{1}\t{2}", Time, LogLevel, Log);
        }
    }
}
