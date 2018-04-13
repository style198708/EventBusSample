using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Bus.Common;
using Bus.Pass;

namespace EventWeb
{
    public class Startup
    {
        private readonly ILogger logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            this.logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.logger.LogInformation("正在对服务进行配置....");
            this.logger.LogInformation($"IServiceCollection集合 Hash Code: {services.GetHashCode()}.");


            services.AddMvc();
            services.AddTransient<IEventHandler, CustomerCreatedEventHandler>();
            services.AddTransient<IMessageSerializer, MessageSerializer>();
            services.AddTransient<IEventStore>(serviceProvider => new DapperEventStore(
                Configuration["mssql:connectionString"],
                serviceProvider.GetRequiredService<ILogger<DapperEventStore>>(),
                serviceProvider.GetRequiredService<IMessageSerializer>()
                ));

            //var eventHandlerExecutionContext = new EventHandlerExecutionContext(services, sc => sc.BuildServiceProvider(), serviceProvider.GetRequiredService<ILogger<DapperEventStore>>());
            services.AddSingleton<IEventHandlerExecutionContext>(
                 serviceProvider => new EventHandlerExecutionContext(services, sc => sc.BuildServiceProvider(), serviceProvider.GetRequiredService<ILogger<EventHandlerExecutionContext>>()
                 ));
            services.AddSingleton<IEventBus, PassThroughEventBus>();

            //services.AddSingleton<IEventBus>(serviceProvider => new PassThroughEventBus(
            //     serviceProvider.GetRequiredService<IEnumerable<IEventHandler>>(),
            //     serviceProvider.GetRequiredService<ILogger<PassThroughEventBus>>()
            //    ));

            this.logger.LogInformation("服务配置完成，已注册到IoC容器！");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            ///增加订阅者的操作
            eventBus.Subscribe<CustomerCreatedEvent, CustomerCreatedEventHandler>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "api/{controller}/{action}/{id?}",
                   defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
