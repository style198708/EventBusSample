using Bus.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bus.Pass
{
    internal sealed class EventQueue
    {
        public event System.EventHandler<EventProcessedEventArgs> EventPushed;
        public EventQueue() { }

        public void Push(IEvent @event)
        {
            this.EventPushed?.Invoke(this, new EventProcessedEventArgs(@event));
        }
    }
}
