using System;

namespace CloudDaemon.Common.Interfaces
{
    public interface ILogger
    {
        void Log(string message);

        void Log(Exception ex);
    }
}
