using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Impl;
using System.Collections.Specialized;

namespace CloudDaemon.Monitors.Taxes
{
    public class TaxesAuthentifier : BaseAuthentifier
    {
        protected override string LoginUrl { get { return StartUrlAfterRedirect; } }

        protected override string StartUrl { get { return "https://cfspart.impots.gouv.fr/portal/dgi/public/perso"; } }

        public TaxesAuthentifier(Profile profile)
            : base(profile)
        {
        }

        protected override NameValueCollection GetSubmittedValues()
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("LMDP_Spi", Profile.Login);
            values.Add("LMDP_Password", Profile.Password);
            return values;
        }
    }
}
