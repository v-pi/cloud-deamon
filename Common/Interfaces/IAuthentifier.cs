using System.Net;

namespace CloudDaemon.Common.Interfaces
{
    public interface IAuthentifier
    {
        WebClient GetAuthenticatedClient();
    }
}