using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CloudDaemon.Monitors.Tennis
{
    public struct AvailableTennisSlot
    {
        public const string StringFormat = "Le court n°{0} ({1}) est disponible le {2:dd/MM/yyyy} à partir de {2:HH\\hmm}.";

        public string Place { get; private set; }

        public DateTime StartDateTime { get; private set; }

        public int CourtNumber { get; private set; }

        public AvailableTennisSlot(HtmlNode html)
        {
            HtmlNodeCollection tableRows = html.SelectNodes("td");
            Place = tableRows[0].InnerText;

            string start = tableRows[2].InnerText + tableRows[3].InnerText;
            StartDateTime = DateTime.ParseExact(start, "dd/MM/yyyyHH\\hmm", CultureInfo.GetCultureInfo("fr-FR"));
            Regex regex = new Regex("Court n°(\\d{1,2})");
            Match match = regex.Match(tableRows[4].InnerText);
            CourtNumber = Int32.Parse(match.Groups[1].Value);
        }

        public override string ToString()
        {
            return String.Format(StringFormat, CourtNumber, Place, StartDateTime);
        }
    }
}
