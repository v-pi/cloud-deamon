using CloudDaemon.Common.Exceptions;
using CloudDaemon.Common.Impl;
using CloudDaemon.Common.Interfaces;
using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace CloudDaemon.Monitors.Free
{
    public class FreeMobileMonitor : AuthentifiedMonitor
    {
        public override string Url { get { return "https://mobile.free.fr/moncompte/index.php?page=suiviconso"; } }
        public decimal RemainingData { get; private set; }

        public override void Monitor()
        {
            IAuthentifier authentifier = new FreeMobileAuthentifier(Profile);
            WebClient client = authentifier.GetAuthenticatedClient();

            string response = client.DownloadString(Url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            HtmlNodeCollection elements = doc.DocumentNode.SelectNodes("//div[@class='progress-text']");

            if (elements == null || elements.Count() != 1)
            {
                throw new HtmlStructureChangedException(Url, "Expected one element with the class 'progress-text'");
            }
            string result = elements.ElementAt(0).InnerText;
            Regex regex = new Regex("reste ([\\d\\.]+) Mio");
            if (!regex.IsMatch(result))
            {
                throw new HtmlStructureChangedException(Url, "Result string should formatted like this : 'reste ([\\d\\.]+) Mio'");
            }

            FreeMobileConsumption consumption = new FreeMobileConsumption()
            {
                RemainingData = Decimal.Parse(regex.Match(result).Groups[1].Value, CultureInfo.InvariantCulture)
            };

            // TODO : Register SQL save
            // TODO : return if no change since last save

            OnMonitorEnded(consumption);
        }
    }
}
