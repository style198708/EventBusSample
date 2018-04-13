using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bus.Common
{
    public abstract class BaseEventBus : IEventBus
    {
        protected readonly IEventHandlerExecutionContext eventHandlerExecutionContext;

        public BaseEventBus(IEventHandlerExecutionContext eventHandlerExecutionContext)
        {
            this.eventHandlerExecutionContext = eventHandlerExecutionContext;
        }


        public abstract Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;


        public abstract void Subscribe<TEvent, TEventHandler>() 
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>;

        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {

            if (!disposedValue)
            {
                if (disposing)
                {
                  
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
         
            Dispose(true);

        }
    }
}
