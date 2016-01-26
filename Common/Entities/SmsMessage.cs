using CloudDaemon.Common.Interfaces;

namespace CloudDaemon.Common.Entities
{
    public class SmsMessage : IHasMessage
    {
        public string Message { get; set; }
    }
}
