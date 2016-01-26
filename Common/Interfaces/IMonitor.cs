using System;

namespace CloudDaemon.Common.Interfaces
{
    public interface IMonitor
    {
        event EventHandler<object> MonitorEnded;

        void Monitor();
    }
}
