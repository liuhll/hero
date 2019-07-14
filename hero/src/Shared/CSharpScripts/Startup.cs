using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.CPlatform.Utilities;

namespace Surging.Hero.ServiceHost
{
    public class Startup
    {
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
    }
}