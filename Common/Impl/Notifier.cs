using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;

namespace CloudDaemon.Common.Impl
{
    public abstract class Notifier : IResultHandler
    {
        public Profile Profile { get; set; }

        public abstract void HandleResult(object sender, object result);
    }
}
