using CloudDaemon.Common.Interfaces;
using System;

namespace CloudDaemon.Common.Impl
{
    public abstract class BaseMonitor : IMonitor
    {
        public abstract string Url { get; }

        public event EventHandler<object> MonitorEnded;

        protected virtual void OnMonitorEnded(object result)
        {
            if (MonitorEnded != null)
            {
                MonitorEnded(this, result);
            }
        }

        public abstract void Monitor();
    }
}
