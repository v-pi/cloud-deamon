using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Impl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CloudDaemon.Monitors.Free
{
    /// <summary>
    /// This class handles the authentication on mobile.free.fr
    /// </summary>
    public class FreeMobileAuthentifier : BaseAuthentifier
    {
        protected override string StartUrl { get { return "https://mobile.free.fr/moncompte/index.php?page=home"; } }
        protected override string LoginUrl { get { return "https://mobile.free.fr/moncompte/index.php?page=home"; } }

        protected bool IsRetry = false;

        public FreeMobileAuthentifier(Profile profile)
            :base(profile)
        {
        }

        protected override NameValueCollection GetSubmittedValues()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(StartPageHtml);
            HtmlNodeCollection elements = doc.DocumentNode.SelectNodes("//img[@class='ident_chiffre_img pointer']");
            
            List<FreeVirtualKey> keys = elements.Select((x, i) => new FreeVirtualKey(i, x.Attributes["src"].Value)).ToList();
            keys.ForEach(k => k.InitKey(Client));

            for (int ii = 0; ii < 10; ii++)
            {
                if (keys.Count(k => k.Key == ii) != 1)
                {
                    if (IsRetry)
                    {
                        throw new Exception(String.Format("Could not find a virtual key corresponding to {0} even after retry.", ii));
                    }
                    else
                    {
                        IsRetry = true;
                        LogManager.Log(String.Format("Could not find a virtual key corresponding to {0}. Retrying...", ii));
                        DownloadStartPage();
                        return GetSubmittedValues();
                    }
                }
            }

            StringBuilder ObsfuscatedLogin = new StringBuilder();
            foreach (int i in Profile.Login.Select(c => Char.GetNumericValue(c)))
            {
                ObsfuscatedLogin.Append(keys.First(k => k.Key == i).Order);
            }

            string token = doc.DocumentNode.SelectNodes("//input[@name='token']").First().Attributes["value"].Value;

            NameValueCollection values = new NameValueCollection();
            values.Add("token", token);
            values.Add("login_abo", ObsfuscatedLogin.ToString());
            values.Add("pwd_abo", Profile.Password);
            return values;
        }
    }
}
