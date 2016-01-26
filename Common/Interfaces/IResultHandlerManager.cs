using CloudDaemon.Common.Entities;
using System.Collections.Generic;

namespace CloudDaemon.Common.Interfaces
{
    public interface IResultHandlerManager
    {
        IEnumerable<ResultHandlerEntity> GetResultHandlers(int IdMonitor);
    }
}
