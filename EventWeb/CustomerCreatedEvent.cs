using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bus.Common;
using Microsoft.Extensions.Logging;

namespace EventWeb
{
    public class CustomerCreatedEvent : IEvent
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }

        public string CustomerName { get; }

        public CustomerCreatedEvent(string customerName)
        {
            this.Id = Guid.NewGuid();
            this.Timestamp = DateTime.Now;
            this.CustomerName = customerName;
        }
    }
    public class CustomerCreatedEventHandler : IEventHandler<CustomerCreatedEvent>
    {
        private readonly IEventStore eventStore;
        private readonly ILogger logger;
        public CustomerCreatedEventHandler(IEventStore eventStore,ILogger<CustomerCreatedEventHandler> logger)
        {
            this.logger = logger;
            this.eventStore = eventStore;
            this.logger.LogInformation($"CustomerCreatedEventHandler构造函数调用完成。Hash Code: {this.GetHashCode()}.");
        }

        public bool CanHandle(IEvent @event)
        {
            return @event.GetType().Equals(typeof(CustomerCreatedEvent));
        }

        public Task<bool> HandleAsync(CustomerCreatedEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation($"开始处理CustomerCreatedEvent事件，处理器Hash Code：{this.GetHashCode()}.");
            this.eventStore.SaveEventAsync(@event);
            this.logger.LogInformation($"结束处理CustomerCreatedEvent事件，处理器Hash Code：{this.GetHashCode()}.");
            return Task.FromResult(true);
        }

        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            return CanHandle(@event) ? HandleAsync((CustomerCreatedEvent)@event, cancellationToken) : Task.FromResult(false);
        }
    }
}
