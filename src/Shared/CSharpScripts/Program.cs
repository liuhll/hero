using System;
using Autofac;
using Microsoft.Extensions.Logging;
using Surging.Cloud.Caching.Configurations;
using Surging.Cloud.Consul.Configurations;
using Surging.Cloud.CPlatform;
using Surging.Cloud.CPlatform.Configurations;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.EventBusRabbitMQ.Configurations;
using Surging.Cloud.ProxyGenerator;
using Surging.Cloud.ServiceHosting;
using Surging.Cloud.ServiceHosting.Internal.Implementation;
using Surging.Cloud.System.Intercept;
using Surging.Cloud.Zookeeper.Configurations;
using SurgingConfig = Surging.Cloud.CPlatform.AppConfig;

namespace Surging.Hero.ServiceHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new ServiceHostBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddMicroService(option =>
                    {
                        option.AddServiceRuntime()
                            .AddClientProxy()
                            .AddRelateServiceRuntime()
                            .AddConfigurationWatch()
                            .AddServiceEngine(typeof(SurgingServiceEngine))
                            .AddClientIntercepted(typeof(CacheProviderInterceptor))
                            ;

                        builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
                    });
                })
                .ConfigureLogging(loggging =>
                {
                    loggging.AddConfiguration(
                        AppConfig.GetSection("Logging"));
                })
                .UseServer(options => { })
                .UseConsoleLifetime()
                .Configure(build =>
                {
#if DEBUG
                    build.AddCacheFile("${cachePath}|/app/configs/cacheSettings.json", false, true);
                    build.AddCPlatformFile("${surgingPath}|/app/configs/surgingSettings.json", false, true); 
                    build.AddEventBusFile("${eventBusPath}|/app/configs/eventBusSettings.json", optional: false);
                    build.AddConsulFile("${consulPath}|/app/configs/consul.json", false, true);
                    build.AddZookeeperFile("${zookeeperPath}|/app/configs/zookeeper.json", false, true);


#else
                    build.AddCacheFile("${cachePath}|configs/cacheSettings.json", optional: false, reloadOnChange: true);                      
                    build.AddCPlatformFile("${surgingPath}|configs/surgingSettings.json", optional: false,reloadOnChange: true);                    
                 //   build.AddEventBusFile("configs/eventBusSettings.json", optional: false);
                    build.AddConsulFile("configs/consul.json", optional: false, reloadOnChange: true);
                     build.AddZookeeperFile("${zookeeperPath}|configs/zookeeper.json", optional: false, reloadOnChange: true);
#endif
                })
                .UseProxy()
                .UseStartup<Startup>()
                .Build();

            using (host.Run())
            {
                Console.WriteLine($"服务主机启动成功{DateTime.Now}。");

#if DEBUG
                // Startup.InitActions();
#endif
            }
        }
    }
}