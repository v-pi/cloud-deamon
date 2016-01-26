using CloudDaemon.Common.Interfaces;
using System;

namespace CloudDaemon.Common.Impl
{
    public class ConsoleResultHandler : IResultHandler
    {
        public void HandleResult(object sender, object result)
        {
            Console.WriteLine(((IHasMessage)result).Message);
        }
    }
}
