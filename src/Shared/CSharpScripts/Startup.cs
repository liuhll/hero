using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Surging.Cloud.CPlatform.Runtime;
using Surging.Cloud.CPlatform.Runtime.Server;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.ProxyGenerator;

namespace Surging.Hero.ServiceHost
{
    public class Startup
    {
        private const string updateHostActionRoute = "api/action/init";
        private const int hostNameSegmentLength = 3;
        private readonly IConfigurationBuilder _configurationBuilder;

        public Startup(IConfigurationBuilder config)
        {
            _configurationBuilder = config;
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

        internal static void InitActions()
        {
            var serviceProxyProvider = ServiceLocator.GetService<IServiceProxyProvider>();
            var serviceEntryProvider = ServiceLocator.GetService<IServiceEntryProvider>();
            var entries = serviceEntryProvider.GetEntries();
            var logger = ServiceLocator.GetService<ILogger<Startup>>();
            var actions = entries.Select(p => new
            {
                ServiceId = p.Descriptor.Id,
                ServiceHost = GetServiceHost(p.Type.FullName),
                Application = GetApplication(p.Type.FullName),
                WebApi = p.RoutePath,
                Method = string.Join(",", p.Methods),
                Name = p.Descriptor.GetMetadata<string>("GroupName"),
                DisableNetwork = p.Descriptor.GetMetadata<bool>("DisableNetwork"),
                EnableAuthorization = p.Descriptor.GetMetadata<bool>("EnableAuthorization"),
                AllowPermission = p.Descriptor.GetMetadata<bool>("AllowPermission"),
                Developer = p.Descriptor.GetMetadata<string>("Director"),
                Date = GetDevDate(p.Descriptor.GetMetadata<string>("Date"))
            }).ToList();
            var rpcParams = new Dictionary<string, object> {{"actions", actions}};
            try
            {
                var result = serviceProxyProvider.Invoke<string>(rpcParams, updateHostActionRoute, HttpMethod.POST)
                    .Result;
                if (result.IsNullOrEmpty())
                    logger.LogInformation("初始化Action失败");
                else
                    logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        private static DateTime? GetDevDate(string dateStr)
        {
            if (dateStr.IsNullOrEmpty()) return null;
            return Convert.ToDateTime(dateStr);
        }

        private static string GetApplication(string serviceFullName)
        {
            return serviceFullName.Split(".").Last();
        }

        private static string GetServiceHost(string serviceFullName)
        {
            return string.Join('.', serviceFullName.Split(".").Take(hostNameSegmentLength));
        }
    }
}