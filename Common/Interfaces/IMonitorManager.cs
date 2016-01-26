using CloudDaemon.Common.Entities;
using System.Collections.Generic;

namespace CloudDaemon.Common.Interfaces
{
    public interface IMonitorManager
    {
        IEnumerable<MonitorEntity> GetAllMonitors();
    }
}
