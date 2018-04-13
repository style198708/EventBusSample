using Bus.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bus.Pass
{
    public class EventHandlerExecutionContext : IEventHandlerExecutionContext
    {
        private readonly IServiceCollection registry;
        private Func<IServiceCollection, IServiceProvider> serviceProviderFactory;
        private ConcurrentDictionary<Type, List<Type>> registrations = new ConcurrentDictionary<Type, List<Type>>();
        private readonly ILogger logger;

        public EventHandlerExecutionContext(IServiceCollection registry, Func<IServiceCollection, IServiceProvider> serviceProviderFactory,ILogger<EventHandlerExecutionContext> logger)
        {
            this.registry = registry;
            this.logger = logger;
            this.serviceProviderFactory = serviceProviderFactory ?? (sc => registry.BuildServiceProvider());
        }

        public async Task HandleEventAsync(IEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            var eventType = @event.GetType();
            if (this.registrations.TryGetValue(eventType, out List<Type> handlerTypes) && handlerTypes?.Count > 0)
            {
                var serviceProvider = this.serviceProviderFactory(this.registry);
                using (var childScope = serviceProvider.CreateScope())
                {
                    foreach(var handlerType in handlerTypes)
                    {
                        var handler = (IEventHandler)childScope.ServiceProvider.GetRequiredService(handlerType);
                        if(handler.CanHandle(@event))
                        {
                            await handler.HandleAsync(@event, cancellationToken);
                        }
                    }
                }

            }
        }

        public bool HandlerRegistered<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        => this.HandlerRegistered(typeof(TEvent), typeof(THandler));

        public bool HandlerRegistered(Type eventType, Type handlerType)
        {
           if(this.registrations.TryGetValue(eventType, out List<Type> handlerTypeList))
            {
                return handlerTypeList != null && handlerTypeList.Contains(handlerType);
            }
            return false;
        }

        public void RegisterHandler<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        => this.RegisterHandler(typeof(TEvent), typeof(THandler));

        public void RegisterHandler(Type eventType, Type handlerType)
        {
            Utils.ConcurrentDictionarySafeRegister(eventType, handlerType, this.registrations);
            this.registry.AddTransient(handlerType);
            this.logger.LogInformation($"IServiceCollection集合 Hash Code: {registry.GetHashCode()}.");
        }
    }
}
