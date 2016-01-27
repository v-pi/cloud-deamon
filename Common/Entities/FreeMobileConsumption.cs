using CloudDaemon.Common.Interfaces;
using System;

namespace CloudDaemon.Common.Entities
{
    public class FreeMobileConsumption : IHasMessage
    {
        private const string MessageFormat = "Remaining data : {0}/{1}Mo\nRemaining Voice : {2:hh'h'mm'm'}\nNext credit : {3:dd/MM/yyyy}";

        public Profile User { get; set; }

        private const decimal TotalData = 50;

        private static TimeSpan TotalVoice = new TimeSpan(2, 0, 0);

        public DateTime StartDate { get; set; }

        public decimal ConsumedData { get; set; }

        public decimal RemainingData
        {
            get
            {
                return TotalData - ConsumedData;
            }
            set
            {
                ConsumedData = TotalData - value;
            }
        }

        public TimeSpan ConsumedVoice { get; set; }

        public TimeSpan RemainingVoice
        {
            get
            {
                return TotalVoice - ConsumedVoice;
            }
            set
            {
                ConsumedVoice = TotalVoice - value;
            }
        }

        public bool IsDataNearMax
        {
            get
            {
                return ConsumedData / TotalData > 0.9m;
            }
        }

        public bool IsVoiceNearMax
        {
            get
            {
                return ConsumedVoice.TotalSeconds / TotalVoice.TotalSeconds > 0.9d;
            }
        }

        public string Message
        {
            get
            {
                return String.Format(MessageFormat, RemainingData, TotalData, RemainingVoice, StartDate.AddMonths(1));
            }
        }
    }
}
