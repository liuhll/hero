using System;
using Microsoft.Extensions.Logging;
using Autofac;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting.Internal.Implementation;
using Surging.Core.ServiceHosting;
using Surging.Core.Caching.Configurations;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.Consul.Configurations;
using Surging.Core.Schedule.Configurations;
using Surging.Core.System.Intercept;

namespace Hl.ServiceHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new ServiceHostBuilder()
                .RegisterServices(builder =>
                {
                    // 注册服务
                    builder.AddMicroService(option =>
                    {
                        // 增加微服务
                        option.AddServiceRuntime()
                            .AddRelateService()
                            .AddConfigurationWatch()
                            .AddClientIntercepted(typeof(CacheProviderInterceptor))
                            .AddServiceEngine(typeof(SurgingServiceEngine));
                            
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
                    build.AddCacheFile("${cachePath}|/app/configs/cacheSettings.json", optional: false,
                        reloadOnChange: true);
                    build.AddCPlatformFile("${srcpPath}|/app/configs/surgingSettings.json", optional: false,
                        reloadOnChange: true);
                    build.AddEventBusFile("${eventBusPath}|/app/configs/eventBusSettings.json", optional: false);
                    build.AddConsulFile("${consulPath}|/app/configs/consul.json", optional: false, reloadOnChange: true);
                    build.AddQuartzFile("${scheduleConfigPath}|/app/configs/scheduleConfig.json", optional: false, reloadOnChange: true);
#else
                    build.AddCacheFile("${cachePath}|Configs/cacheSettings.json", optional: false,
                        reloadOnChange: true);
                    build.AddCPlatformFile("${srcpPath}|Configs/surgingSettings.json", optional: false,
                        reloadOnChange: true);
                    build.AddEventBusFile("${eventBusPath}|Configs/eventBusSettings.json", optional: false);
                    build.AddConsulFile("${consulPath}|Configs/consul.json", optional: false, reloadOnChange: true);
                    build.AddQuartzFile("Configs/scheduleConfig.json", optional: false, reloadOnChange: true);
#endif

                })
                .UseProxy()
                .UseStartup<Startup>().Build();

            using (host.Run())
            {
                Console.WriteLine($"服务端启动成功，{DateTime.Now}。");
            }
        }
    }
}