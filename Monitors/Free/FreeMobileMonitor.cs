using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Exceptions;
using CloudDaemon.Common.Impl;
using CloudDaemon.Common.Interfaces;
using CloudDaemon.DAL;
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

            HtmlNodeCollection activeElements = doc.DocumentNode.SelectNodes("//span[@class='actif']");
            HtmlNodeCollection progressElements = doc.DocumentNode.SelectNodes("//div[@class='progress-text']");

            if (progressElements == null || progressElements.Count() != 1)
            {
                throw new HtmlStructureChangedException(Url, "Expected one element with the class 'progress-text'");
            }
            string result = progressElements.ElementAt(0).InnerText;
            Regex regex = new Regex("reste ([\\d\\.]+) Mio");
            if (!regex.IsMatch(result))
            {
                throw new HtmlStructureChangedException(Url, "Result string should formatted like this : 'reste ([\\d\\.]+) Mio'");
            }

            FreeMobileConsumption consumption = new FreeMobileConsumption()
            {
                RemainingData = Decimal.Parse(regex.Match(result).Groups[1].Value, CultureInfo.InvariantCulture),
                StartDate = DateTime.ParseExact(activeElements[0].InnerText, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                ConsumedVoice = TimeSpan.ParseExact(activeElements[2].InnerText, "h'h'm'min'ss's'", CultureInfo.InvariantCulture)
            };

            FreeMobileRepository repository = new FreeMobileRepository();
            FreeMobileConsumption previous = repository.GetLastConsumption(Profile.IdProfile);
            repository.SaveConsumption(consumption);

            if ((consumption.IsDataNearMax && !previous.IsDataNearMax) // if the new data consumption is almost over when the previous wasn't
                || consumption.IsVoiceNearMax && !previous.IsVoiceNearMax) // OR if the new voice consumption is almost over when the previous wasn't
            {
                OnMonitorEnded(consumption);
            }
        }
    }
}
