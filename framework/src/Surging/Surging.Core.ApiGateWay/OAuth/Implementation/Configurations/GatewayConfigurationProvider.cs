using Microsoft.Extensions.Configuration;
using Surging.Core.CPlatform.Configurations.Remote;
using System.IO;

namespace Surging.Core.ApiGateWay.OAuth.Implementation.Configurations
{
    public class GatewayConfigurationProvider : FileConfigurationProvider
    {
        public GatewayConfigurationProvider(GatewayConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            var parser = new JsonConfigurationParser();
            this.Data = parser.Parse(stream, null);
        }
    }
}