using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bus.Common
{
    public interface IEventHandler
    {
        Task<bool> HandleAsync(IEvent @event,CancellationToken cancellationToken = default);
        bool CanHandle(IEvent @event);
    }

    public interface IEventHandler<in T>:IEventHandler
    {
        Task<bool> HandleAsync(T @event, CancellationToken cancellationToken = default);
    }
}
