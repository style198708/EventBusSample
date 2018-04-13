using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bus.Common;
using Microsoft.Extensions.Logging;

namespace EventWeb.Controllers
{
    public class ValuesController : Controller
    {

        private readonly IEventBus eventBus;
        private readonly ILogger logger;

        public ValuesController(IEventBus eventBus,ILogger<ValuesController> logger)
        {
            this.logger = logger;
            this.eventBus = eventBus;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost]
        public async Task<bool> Create()
        {
            this.logger.LogInformation($"开始创建客户信息。");
            await this.eventBus.PublishAsync(new CustomerCreatedEvent("这是一个测试"));
            this.logger.LogInformation($"客户信息创建成功。");
            return true;
        }

        // POST api/values
        [HttpPost]
        public void Post()
        {
            this.eventBus.PublishAsync(new CustomerCreatedEvent("这是一个测试"));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
