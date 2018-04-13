using System;
using System.Collections.Generic;
using System.Text;

namespace Bus.Common
{
    public class EventProcessedEventArgs
    {
        public IEvent Event { get; }

        public EventProcessedEventArgs(IEvent @event)
        {
            this.Event = @event;
        }
    }
}
