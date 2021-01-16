using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Surging.Cloud.Caching.Configurations;
using Surging.Cloud.CPlatform;
using Surging.Cloud.CPlatform.Configurations;
using Surging.Cloud.EventBusRabbitMQ.Configurations;
using Surging.Cloud.Zookeeper.Configurations;

namespace Surging.Hero.ServiceHost
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .RegisterMicroServices()
                .ConfigureAppConfiguration((hostContext, configure) =>
                {
#if DEBUG
                    configure.AddCacheFile("${cachePath}|/app/configs/cacheSettings.json", optional: false, reloadOnChange: true);
                    configure.AddCPlatformFile("${surgingPath}|/app/configs/surgingSettings.json", optional: false,
                        reloadOnChange: true);
                    configure.AddEventBusFile("${eventBusPath}|/app/configs/eventBusSettings.json", optional: false, reloadOnChange: true);
                    // configure.AddConsulFile("${consulPath}|/app/configs/consul.json", optional: false, reloadOnChange: true);
                    configure.AddZookeeperFile("${zookeeperPath}|/app/configs/zookeeper.json", optional: false,
                        reloadOnChange: true);
#else
                    configure.AddCacheFile("${cachePath}|configs/cacheSettings.json", optional: false, reloadOnChange: true);
                    configure.AddCPlatformFile("${surgingPath}|configs/surgingSettings.json", optional: false, reloadOnChange: true);
                    configure.AddEventBusFile("${eventBusPath}|configs/eventBusSettings.json", optional: false);
                    configure.AddConsulFile("${consulPath}|configs/consul.json", optional: false, reloadOnChange: true);
                    configure.AddZookeeperFile("${zookeeperPath}|configs/zookeeper.json", optional: false, reloadOnChange: true);
#endif
                })
                .UseServer()
                .UseClient()
                .Build().RunAsync();
        }
    }
}