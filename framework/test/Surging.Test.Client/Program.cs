using Autofac;
using Microsoft.Extensions.Logging;
using Surging.Core.Caching;
using Surging.Core.Caching.Configurations;
using Surging.Core.Consul.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using System;

namespace Surging.Test.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHostBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddMicroService(option =>
                    {
                        option.AddClient()
                        .AddCache();
                        builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
                    });
                })
                .ConfigureLogging(logger =>
                {
                    logger.AddConfiguration(
                        Core.CPlatform.AppConfig.GetSection("Logging"));
                })
                .Configure(build => {
                    {
#if DEBUG
                        build.AddCacheFile("${cachePath}|/app/configs/cacheSettings.json", optional: false, reloadOnChange: true);
                        build.AddCPlatformFile("${surgingPath}|/app/configs/surgingSettings.json", optional: false, reloadOnChange: true);
                        build.AddEventBusFile("${eventBusPath}|/app/configs/eventBusSettings.json", optional: false);
                        build.AddConsulFile("${consulPath}|/app/configs/consul.json", optional: false, reloadOnChange: true);


#else
                    build.AddCacheFile("${cachePath}|configs/cacheSettings.json", optional: false, reloadOnChange: true);                      
                    build.AddCPlatformFile("${surgingPath}|configs/surgingSettings.json", optional: false,reloadOnChange: true);                    
                    build.AddEventBusFile("configs/eventBusSettings.json", optional: false);
                    build.AddConsulFile("configs/consul.json", optional: false, reloadOnChange: true);
#endif
                    }
                })
                .UseClient()
                .UseProxy()
                .UseStartup<Startup>()
                .Build();

            using (host.Run())
            {
                Console.WriteLine($"服务调用者启动成功，{DateTime.Now}。");
            }
        }

    }
}
