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


        public FreeMobileAuthentifier(Profile profile)
            :base(profile)
        {
        }

        protected override NameValueCollection GetSubmittedValues()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(StartPageHtml);
            HtmlNodeCollection elements = doc.DocumentNode.SelectNodes("//img[@class='ident_chiffre_img pointer']");

            // TODO : Check that the 10 figures were successfully identified (otherwise, retry ? try and improve success rate ?)
            List<FreeVirtualKey> keys = elements.Select((x, i) => new FreeVirtualKey(i, x.Attributes["src"].Value)).ToList();
            keys.ForEach(k => k.InitKey(Client));

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
