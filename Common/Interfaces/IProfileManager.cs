using CloudDaemon.Common.Entities;

namespace CloudDaemon.Common.Interfaces
{
    public interface IProfileManager
    {
        Profile GetProfileById(int profileId);
    }
}
