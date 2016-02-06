using CloudDaemon.Common.Interfaces;
using System.Net;
using System.Collections.Specialized;
using CloudDaemon.Common.Entities;

namespace CloudDaemon.Common.Impl
{
    public abstract class BaseAuthentifier : IAuthentifier
    {
        protected abstract string StartUrl { get; }
        protected abstract string LoginUrl { get; }
        protected string StartUrlAfterRedirect { get; set; }
        protected Profile Profile;

        protected SmartWebClient Client;
        protected string StartPageHtml;

        public BaseAuthentifier(Profile profile)
        {
            this.Client = new SmartWebClient();
            this.Profile = profile;
        }

        public virtual WebClient GetAuthenticatedClient()
        {
            DownloadStartPage();
            SubmitLoginAndPassword();
            return Client;
        }

        protected virtual void DownloadStartPage()
        {
            StartPageHtml = Client.DownloadString(StartUrl);
            StartUrlAfterRedirect = Client.ResponseUri.ToString();
        }

        protected virtual NameValueCollection GetSubmittedValues()
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("login", Profile.Login);
            values.Add("password", Profile.Password);
            return values;
        }

        protected virtual void SubmitLoginAndPassword()
        {
            NameValueCollection values = GetSubmittedValues();
            Client.UploadValues(LoginUrl, "POST", values);
        }
    }
}
