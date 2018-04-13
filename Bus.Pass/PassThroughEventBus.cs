using Bus.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bus.Pass
{
    public sealed class PassThroughEventBus : BaseEventBus
    {
        private readonly EventQueue eventQueue = new EventQueue();
        private readonly ILogger logger;
        //private readonly IEnumerable<IEventHandler> eventHandlers;
   


        public PassThroughEventBus(IEventHandlerExecutionContext context,ILogger<PassThroughEventBus> logger):base(context)
        {
            this.logger = logger;
           
            logger.LogInformation($"PassThroughEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");
            eventQueue.EventPushed += EventQueue_EventPushed;
        }


        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => eventQueue.Push(@event));
        }

        /// <summary>
        /// 订阅
        /// </summary>
        public override void Subscribe<TEvent,TEventHandler>() 
        {
           if(!this.eventHandlerExecutionContext.HandlerRegistered<TEvent,TEventHandler>())
            {
                this.eventHandlerExecutionContext.RegisterHandler<TEvent, TEventHandler>();
            }
        }

        private async void EventQueue_EventPushed(object sender, EventProcessedEventArgs e)
        {
            //(from eh in this.eventHandlers where eh.CanHandle(e.Event) select eh).ToList().ForEach(
            //     async eh => await eh.HandleAsync(e.Event));
            await this.eventHandlerExecutionContext.HandleEventAsync(e.Event);
        }

        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.eventQueue.EventPushed -= EventQueue_EventPushed;
                }
                logger.LogInformation($"PassThroughEventBus已经被Dispose。Hash Code:{this.GetHashCode()}.");
                disposedValue = true;
            }
        }
    }
}
