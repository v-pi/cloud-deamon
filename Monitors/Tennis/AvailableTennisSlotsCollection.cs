using CloudDaemon.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace CloudDaemon.Monitors.Tennis
{
    public class AvailableTennisSlotsCollection : IHasSubject, IHasMessage, IEnumerable<AvailableTennisSlot>
    {
        public const string SubjectFormat = "Nouvelles disponibilités pour le tennis";

        public IEnumerable<AvailableTennisSlot> Slots { get; set; }

        public AvailableTennisSlotsCollection(IEnumerable<AvailableTennisSlot> slots)
        {
            Slots = slots.ToList();
        }

        public string Subject
        {   
            get
            {
                return SubjectFormat;
            }
        }

        public string Message
        {
            get
            {
                return String.Join("\n", Slots);
            }
        }

        public IEnumerator<AvailableTennisSlot> GetEnumerator()
        {
            return Slots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Slots.GetEnumerator();
        }
    }
}
