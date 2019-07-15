using Microsoft.Extensions.Configuration;

namespace Surging.Core.ApiGateWay.OAuth.Implementation.Configurations
{
    public class GatewayConfigurationSource : FileConfigurationSource
    {
        public string ConfigurationKeyPrefix { get; set; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new GatewayConfigurationProvider(this);
        }
    }
}