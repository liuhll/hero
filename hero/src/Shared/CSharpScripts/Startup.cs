using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;

namespace Surging.Hero.ServiceHost
{
    public class Startup
    {
        private readonly IConfigurationBuilder _configurationBuilder;
        private const string updateHostActionRoute = "v1/api/action/initactions";
        private const int hostNameSegmentLength = 3;

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
            var actions = entries.Select(p => new {
                ServiceId = p.Descriptor.Id,
                ServiceHost = GetServiceHost(p.Type.FullName),
                Application = GetApplication(p.Type.FullName),
                WebApi = p.RoutePath,
                Name = p.Descriptor.GetMetadata<string>("GroupName"),
                DisableNetwork = p.Descriptor.GetMetadata<bool>("DisableNetwork"),
                EnableAuthorization = p.Descriptor.GetMetadata<bool>("EnableAuthorization"),
                AllowPermission = p.Descriptor.GetMetadata<bool>("AllowPermission"),
                Developer = p.Descriptor.GetMetadata<string>("Director"), 
                Date = GetDevDate(p.Descriptor.GetMetadata<string>("Date"))

            }).ToList();
            var rpcParams = new Dictionary<string, object>() { { "actions", actions } };
            try
            {
                var result = serviceProxyProvider.Invoke<string>(rpcParams, updateHostActionRoute).Result;
                if (result.IsNullOrEmpty())
                {
                    logger.LogInformation("≥ı ºªØAction ß∞‹");
                }
                else
                {
                    logger.LogInformation(result);
                }
            }
            catch (Exception ex) {
                logger.LogError(ex.Message,ex);
            }
          
        }

        private static DateTime? GetDevDate(string dateStr)
        {
            if (dateStr.IsNullOrEmpty())
            {
                return null;
            }
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