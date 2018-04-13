using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Bus.Common
{
    /// <summary>
    /// 事件仓库
    /// </summary>
    public interface IEventStore
    {
        Task SaveEventAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
