using System;

namespace CloudDaemon.Common.Entities
{
    public struct AvailableTennisSlot
    {
        public const string StringFormat = "Le court n°{0} ({1}) est disponible le {2:dd/MM/yyyy} à partir de {2:HH\\hmm}.";

        public string Place { get; private set; }

        public DateTime StartDateTime { get; private set; }

        public int CourtNumber { get; private set; }

        public AvailableTennisSlot(string place, DateTime startDateTime, int courtNumber)
        {
            this.Place = place;
            this.StartDateTime = startDateTime;
            this.CourtNumber = courtNumber;
        }

        public override string ToString()
        {
            return String.Format(StringFormat, CourtNumber, Place, StartDateTime);
        }
    }
}
