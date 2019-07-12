using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Surging.Core.Caching.Configurations;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Surging.Test.Client
{
    public class Startup
    {
        private ContainerBuilder _builder;
        public Startup(IConfigurationBuilder config)
        {
            ConfigureEventBus(config);
            ConfigureCache(config);
        }

        public IContainer ConfigureServices(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            ConfigureLogging(services);
            builder.Populate(services);
            _builder = builder;
            ServiceLocator.Current = builder.Build();
            return ServiceLocator.Current;
        }

        public void Configure(IContainer app)
        {

        }

        /// <summary>
        /// 配置日志服务
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging();
        }

        private static void ConfigureEventBus(IConfigurationBuilder build)
        {
            build
            .AddEventBusFile("eventBusSettings.json", optional: false);
        }

        /// <summary>
        /// 配置缓存服务
        /// </summary>
        private void ConfigureCache(IConfigurationBuilder build)
        {
            build
              .AddCacheFile("cacheSettings.json", optional: false);
        }

    }
}