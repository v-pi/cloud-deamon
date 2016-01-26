using CloudDaemon.Common.Entities;

namespace CloudDaemon.Common.Impl
{
    public abstract class AuthentifiedMonitor : BaseMonitor
    {
        public Profile Profile { get; set; }
    }
}
