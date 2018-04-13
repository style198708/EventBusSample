using System;
using System.Collections.Generic;
using System.Text;

namespace Bus.Common
{
    public interface IEvent
    {
        Guid Id { get;  }
        DateTime Timestamp { get; }
    }
}
