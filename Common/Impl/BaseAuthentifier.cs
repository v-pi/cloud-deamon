using CloudDaemon.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Collections.Specialized;
using CloudDaemon.Common.Entities;

namespace CloudDaemon.Common.Impl
{
    public abstract class BaseAuthentifier : IAuthentifier
    {
        protected abstract string StartUrl { get; }
        protected abstract string LoginUrl { get; }
        protected Profile Profile;

        protected WebClient Client;
        protected string StartPageHtml;

        public BaseAuthentifier(Profile profile)
        {
            this.Client = new WebClient();
            this.Profile = profile;
        }

        public virtual WebClient GetAuthenticatedClient()
        {
            SetCookies();
            SubmitLoginAndPassword();
            return Client;
        }

        protected virtual void SetCookies()
        {
            StartPageHtml = Client.DownloadString(StartUrl);

            IEnumerable<string> cookies = Client.ResponseHeaders[HttpResponseHeader.SetCookie].Split(',').Select(c => c.Split(';')[0]);
            Client.Headers.Add(HttpRequestHeader.Cookie, String.Join("; ", cookies));
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
            Client.Headers[HttpRequestHeader.Referer] = StartUrl;
            Client.UploadValues(LoginUrl, "POST", values);
        }
    }
}
