using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.ApiGateWay.OAuth.Implementation.Configurations;
using Surging.Core.Caching.Configurations;
using ConsulConfigInfo = Surging.Core.Consul.Configurations.ConfigInfo;
using ZookeeperConfigInfo = Surging.Core.Zookeeper.Configurations.ConfigInfo;
using ApiGateWayConfig = Surging.Core.ApiGateWay.AppConfig;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.System.Intercept;
using Surging.Core.ApiGateWay;
using Surging.Core.ApiGateWay.Configurations;
using Surging.Core.Zookeeper;
using Surging.Core.ProxyGenerator;
using Surging.Core.Consul;
using Surging.Core.Caching;
using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform.Cache;
using System.Linq;

namespace Surging.Hero.WebApi
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "hl";
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddCacheFile("configs/cacheSettings.json", optional: false)
                 .AddJsonFile("configs/appsettings.json", optional: true, reloadOnChange: true)
                 .AddGatewayFile("configs/gatewaySettings.json", optional: false)
                 .AddJsonFile($"configs/appsettings.{env.EnvironmentName}.json", optional: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
           
            return RegisterAutofac(services);
        }

        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            var registerConfig = ApiGateWayConfig.Register;
            services.AddMvc(options=> {
                options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options => {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            services.AddLogging();
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    corsBuilder => corsBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.AddMicroService(option =>
            {
                option.AddClient();
                option.AddCache();
                if (registerConfig.Provider == RegisterProvider.Consul)
                {
                    option.UseConsulManager(new ConsulConfigInfo(registerConfig.Address, enableChildrenMonitor: false));
                }
                else if (registerConfig.Provider == RegisterProvider.Zookeeper)
                {
                    option.UseZooKeeperManager(new ZookeeperConfigInfo(registerConfig.Address, enableChildrenMonitor: true));
                }

                option.AddClientIntercepted(typeof(CacheProviderInterceptor));
                option.AddApiGateWay();
                builder.Register(m => new CPlatformContainer(ServiceLocator.Current));
            });
            ServiceLocator.Current = builder.Build();
            return new AutofacServiceProvider(ServiceLocator.Current);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            var serviceCacheProvider = ServiceLocator.Current.Resolve<ICacheNodeProvider>();
            var addressDescriptors = serviceCacheProvider.GetServiceCaches().ToList();
            ServiceLocator.Current.Resolve<IServiceCacheManager>().SetCachesAsync(addressDescriptors);
            ServiceLocator.Current.Resolve<IConfigurationWatchProvider>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
        
            app.UseHttpsRedirection();
            app.UseMvc(routes=> {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                "Path",
                "{*path}",
                new { controller = "Services", action = "Path" });
            });
        }
    }
}
