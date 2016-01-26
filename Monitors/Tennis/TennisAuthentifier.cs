using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Impl;

namespace CloudDaemon.Monitors.Tennis
{
    public class TennisAuthentifier : BaseAuthentifier
    {
        protected override string StartUrl { get { return "https://teleservices.paris.fr/srtm/jsp/web/index.jsp"; } }
        protected override string LoginUrl { get { return "https://teleservices.paris.fr/srtm/authentificationConnexion.action"; } }

        public TennisAuthentifier(Profile profile)
            : base(profile)
        {
        }
    }
}
