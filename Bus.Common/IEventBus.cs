using System;
using System.Collections.Generic;
using System.Text;

namespace Bus.Common
{

    public interface IEventBus : IEventPublisher, IEventSubscriber
    {
    }
}
