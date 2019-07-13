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
using Surging.Test.Client.Clients.Dtos;
using Surging.Test.Server.Demo;
using System;
using System.Collections.Generic;
using AppConfig = Surging.Core.CPlatform.AppConfig;

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
                         option.AddServiceRuntime()
                         .AddClientProxy()
                         .AddRelateServiceRuntime()
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
                 })
                 .UseProxy()
                 .UseStartup<Startup>()
                 .Build();

            using (host.Run())
            {
                Console.WriteLine($"服务调用者启动成功，{DateTime.Now}。");

                TestRpcCallByProxyFactory();
                TestRpcCallByProxyProvider();
            }
        }

        private static void TestRpcCallByProxyFactory()
        {
            var serviceProxyFactory = ServiceLocator.GetService<IServiceProxyFactory>();
            var demoProxy = serviceProxyFactory.CreateProxy<IDemoApplication>("demo.v1");
            var userInfo = demoProxy.GetUserInfo("1").Result;
            DebugCheck.NotNull(userInfo);
        }

        private static void TestRpcCallByProxyProvider()
        {
            var serviceProxyProvider = ServiceLocator.GetService<IServiceProxyProvider>();
            var userInfo = serviceProxyProvider.Invoke<GetUserInfoOutput>(new Dictionary<string, object> { { "userId", "1" } }, "v1/api/demo/getuserinfo", "demo.v1").Result;
            DebugCheck.NotNull(userInfo);
        }


    }
}
