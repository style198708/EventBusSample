using System;
using System.Collections.Generic;
using System.Text;

namespace Bus.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageSerializer
    {
        string SerializeObject(object obj);
    }
}
