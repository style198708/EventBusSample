using System;
using System.Collections.Generic;
using System.Text;

namespace Bus.Common
{
    public interface IEventSubscriber : IDisposable
    {

        void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>;
         
    }
}
