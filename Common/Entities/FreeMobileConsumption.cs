using CloudDaemon.Common.Interfaces;
using System;

namespace CloudDaemon.Common.Entities
{
    public class FreeMobileConsumption : IHasMessage
    {
        private const string MessageFormat = "Remaining data : {0}/{1}Mo";

        private const decimal TotalData = 50;

        public DateTime StartDate { get; set; }

        public decimal ConsumedData { get; set; }

        public Profile User{ get; set; }

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

        public string Message
        {
            get
            {
                return String.Format(MessageFormat, RemainingData, TotalData);
            }
        }
    }
}
