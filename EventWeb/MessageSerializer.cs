using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bus.Common;
using Newtonsoft.Json;

namespace EventWeb
{
    public class MessageSerializer : IMessageSerializer
    {
        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
