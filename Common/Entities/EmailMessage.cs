using CloudDaemon.Common.Interfaces;

namespace CloudDaemon.Common.Entities
{
    public class EmailMessage : IHasMessage, IHasSubject
    {
        public string Message { get; set; }

        public string Subject { get; set; }
    }
}
