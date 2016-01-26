using CloudDaemon.Common.Interfaces;
using CloudDaemon.DAL;

namespace CloudDaemon.Monitors.Tennis
{
    public class TennisResultHandler : IResultHandler
    {
        public void HandleResult(object sender, object result)
        {
            AvailableTennisSlotsCollection results = (AvailableTennisSlotsCollection)result;
            TennisRepository repository = new TennisRepository();
            foreach (AvailableTennisSlot slot in results)
            {
                repository.SaveAvailableSlot(slot.StartDateTime, slot.CourtNumber);
            }
        }
    }
}
