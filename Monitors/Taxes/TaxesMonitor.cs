using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Impl;
using CloudDaemon.Common.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudDaemon.Monitors.Taxes
{
    public class TaxesMonitor : AuthentifiedMonitor
    {
        private const string Domain = "https://cfspart.impots.gouv.fr";

        private const string Url2 = "https://cfspart.impots.gouv.fr/stl/satelit.web";

        public override string Url {  get { return "https://cfspart.impots.gouv.fr/stl/satelit.web?templatename=accueilpartcertificat&mode=paiement"; } }

        private const string NoTaxToPay = "À cette date, aucun impôt payable en ligne n’est rattaché à votre numéro fiscal.";

        public override void Monitor()
        {
            IAuthentifier authentifier = new TaxesAuthentifier(Profile);
            WebClient client = authentifier.GetAuthenticatedClient();

            string redirectPage1 = client.DownloadString(Url);

            Match match = Regex.Match(redirectPage1, "document\\.location\\.replace\\s*\\(\"([^\"]+)\"\\);", RegexOptions.RightToLeft);
            string redirectPage2Url = match.Groups[1].Value;

            client.DownloadString(Domain + redirectPage2Url);

            NameValueCollection values = new NameValueCollection();
            values.Add("templatename", "charpente");
            values.Add("pagedemande", "paiementcertificat1");
            values.Add("numeroscenario", "310");
            values.Add("menu", "");

            string response = Encoding.UTF8.GetString(client.UploadValues(Url2, values));

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);
            
            if (WebUtility.HtmlDecode(response).Contains(NoTaxToPay))
                return;

            HtmlNodeCollection taxesToPay = doc.DocumentNode.SelectNodes("//table[@id='tableaufacturesavisnonpayees1']//tr[@class='cssTableauLigne']");

            foreach(HtmlNode taxNode in taxesToPay)
            {
                TaxNotice notice = new TaxNotice()
                {
                    Id = Int64.Parse(taxNode.ChildNodes[1].InnerText),
                    Amount = Decimal.Parse(WebUtility.HtmlDecode(taxNode.ChildNodes[7].InnerText), NumberStyles.Currency, CultureInfo.GetCultureInfo("fr-FR")),
                    PaymentDate = DateTime.ParseExact(taxNode.ChildNodes[5].InnerText.Trim(), "dd/MM/yyyy", null),
                };

                OnMonitorEnded(notice);
            }
        }
    }
}
