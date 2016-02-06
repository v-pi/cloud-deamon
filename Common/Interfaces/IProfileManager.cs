using CloudDaemon.Common.Entities;

namespace CloudDaemon.Common.Interfaces
{
    public interface IProfileManager
    {
        Profile GetProfileByAlias(string alias);

        void InsertProfile(Profile profile);
    }
}
