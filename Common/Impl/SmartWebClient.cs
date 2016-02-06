using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace CloudDaemon.Common.Impl
{
    [System.ComponentModel.DesignerCategory("Code")] // For some reason, WebClient is a Component which triggers VS to make this class a designer
    public class SmartWebClient : WebClient
    {
        public Uri ResponseUri { get; private set; }

        private Dictionary<string, string> cookies = new Dictionary<string, string>();

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            if (ResponseUri != null)
            {
                this.Headers[HttpRequestHeader.Referer] = ResponseUri.ToString();
            }
            WebResponse response = base.GetWebResponse(request);
            ResponseUri = response.ResponseUri;
            SetCookies();
            return response;
        }

        protected virtual void SetCookies()
        {
            if (this.ResponseHeaders[HttpResponseHeader.SetCookie] != null)
            {
                IEnumerable<string> newCookies = this.ResponseHeaders[HttpResponseHeader.SetCookie].Split(',').Select(c => c.Split(';')[0]);
                foreach (string newCookie in newCookies)
                {
                    string cookieName = newCookie.Split('=')[0];
                    cookies[cookieName] = newCookie;
                }

                this.Headers[HttpRequestHeader.Cookie] = String.Join("; ", cookies.Values);
            }
        }
    }
}