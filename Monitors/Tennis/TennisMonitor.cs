using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Exceptions;
using CloudDaemon.Common.Impl;
using CloudDaemon.Common.Interfaces;
using CloudDaemon.DAL;
using HtmlAgilityPack;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudDaemon.Monitors.Tennis
{
    public class TennisMonitor : AuthentifiedMonitor
    {
        public override string Url { get { return "https://teleservices.paris.fr/srtm/reservationCreneauInit.action"; } }
        private const string SearchUrl = "https://teleservices.paris.fr/srtm/reservationCreneauListe.action";
        private const string NoResult = "Aucun cr&eacute;neau trouv&eacute; pour les crit&egrave;res saisis";

        private readonly NameValueCollection DefaultPreferences;

        public TennisMonitor()
        {
            DefaultPreferences = new NameValueCollection()
            {
                {"provenanceCriteres", "true" },
                {"libellePlageHoraire", "" }, // Soirée (18h-21h)
                {"nomCourt", "" },
                {"actionInterne", "recherche" },
                {"champ", "" },
                {"tennisArrond", "" }, //Jules Ladoumègue@19
                {"arrondissement",  "19" },
                {"arrondissement2", "" },
                {"arrondissement3", "" },
                {"dateDispo", "" },
                {"heureDispo", "" },
                {"plageHoraireDispo", "20@21" },
                {"revetement", "" },
                {"courtEclaire", "on" },
                { "courtCouvert", "on" }
            };
        }

        public override void Monitor()
        {
            IAuthentifier authentifier = new TennisAuthentifier(Profile);
            WebClient client = authentifier.GetAuthenticatedClient();
            byte[] result = client.UploadValues(SearchUrl, "POST", DefaultPreferences);
            string resultString = Encoding.Default.GetString(result);


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(resultString);

            if (resultString.Contains(NoResult))
            {
                LogManager.Log("No results");
                return;
            }

            HtmlNodeCollection elements = doc.DocumentNode.SelectNodes("//table[@class='normal']/tbody/tr");

            if (elements == null || elements.Count() == 0)
            {
                throw new HtmlStructureChangedException(Url, "Expected one or more elements matching xpath : //table[@class='normal']/tbody/tr");
            }
            AvailableTennisSlotsCollection tennisSlots = new AvailableTennisSlotsCollection(
                elements
                    .Select(e => ConvertNodeToTennisSlot(e))
                    .Where(
                        s => s.CourtNumber != 8 &&                                  // Fuck court 8
                        s.Place == "Jules Ladoumègue" &&                            // Fuck this website's encoding
                        s.StartDateTime.DayOfWeek != System.DayOfWeek.Saturday &&   // No tennis on weekends
                        s.StartDateTime.DayOfWeek != System.DayOfWeek.Sunday));     // !

            TennisRepository repository = new TennisRepository();
            tennisSlots.Slots = tennisSlots.Slots.Where(s => !repository.IsAvailableSlotAlreadySent(s.StartDateTime));

            if (tennisSlots.Slots.Count() > 0)
            {
                OnMonitorEnded(tennisSlots);
            }
        }

        private static AvailableTennisSlot ConvertNodeToTennisSlot(HtmlNode html)
        {
            HtmlNodeCollection tableRows = html.SelectNodes("td");
            string place = tableRows[0].InnerText;
            string start = tableRows[2].InnerText + tableRows[3].InnerText;
            DateTime startDateTime = DateTime.ParseExact(start, "dd/MM/yyyyHH\\hmm", CultureInfo.GetCultureInfo("fr-FR"));
            Regex regex = new Regex("Court n°(\\d{1,2})");
            Match match = regex.Match(tableRows[4].InnerText);
            int courtNumber = Int32.Parse(match.Groups[1].Value);

            return new AvailableTennisSlot(place, startDateTime, courtNumber);
        }
    }
}
