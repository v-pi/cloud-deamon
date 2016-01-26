using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System;
using System.Net;

namespace CloudDaemon.Common.Impl
{
    public class SmsNotifier : Notifier
    {
        private const string Url = "https://smsapi.free-mobile.fr/sendmsg?user={0}&pass={1}&msg={2}";

        public override void HandleResult(object sender, object result)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadData(String.Format(
                    Url,
                    Profile.Login,
                    Profile.Password,
                    WebUtility.HtmlEncode(((IHasMessage)(result)).Message)));
            }
        }
    }
}
