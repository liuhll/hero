using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using System.Collections.Generic;
using System.Linq;

namespace Surging.Test.Server
{
    public class Startup
    {
        private const string updateHostActionRoute = "v1/api/action/updateappactions";
        private const int hostNameSegmentLength = 3;
        public Startup(IConfigurationBuilder config)
        {

        }

        public IContainer ConfigureServices(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            builder.Populate(services);
            ServiceLocator.Current = builder.Build();
            return ServiceLocator.Current;
        }

        public void Configure(IContainer app)
        {
   
        }
        internal static void UpdateHostActions()
        {
            var serviceProxyProvider = ServiceLocator.GetService<IServiceProxyProvider>();
            var serviceEntryProvider = ServiceLocator.GetService<IServiceEntryProvider>();
            var entries = serviceEntryProvider.GetEntries();
            var actions = entries.Select(p => new {
                ServiceHost = GetServiceHost(p.Type.FullName),
                Application = GetApplication(p.Type.FullName),
                WebApi = p.RoutePath,
                Name = p.Descriptor.GetMetadata<string>("Name"),
                DisableNetwork = p.Descriptor.GetMetadata<bool>("DisableNetwork"),
                EnableAuthorization = p.Descriptor.GetMetadata<bool>("EnableAuthorization"),
                AllowPermission = p.Descriptor.GetMetadata<bool>("AllowPermission"),

            }).ToList();
            var rpcParams = new Dictionary<string, object>() { { "actions", actions.ToList() } };
            var result = serviceProxyProvider.Invoke<string>(rpcParams, updateHostActionRoute).Result;
        }

        private static string GetApplication(string serviceFullName)
        {
           return serviceFullName.Split(".")[hostNameSegmentLength];
        }

        private static string GetServiceHost(string serviceFullName)
        {
            return string.Join('.', serviceFullName.Split(".").Take(hostNameSegmentLength));
        }
    }
}
