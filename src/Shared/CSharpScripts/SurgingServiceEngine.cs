using Surging.Cloud.CPlatform.Engines.Implementation;
using Surging.Cloud.CPlatform.Utilities;

namespace Surging.Hero.ServiceHost
{
    public class SurgingServiceEngine : VirtualPathProviderServiceEngine
    {
        public SurgingServiceEngine()
        {
            ModuleServiceLocationFormats = new[]
            {
                EnvironmentHelper.GetEnvironmentVariable("${ModulePath1}|Modules")
            };
            ComponentServiceLocationFormats = new[]
            {
                EnvironmentHelper.GetEnvironmentVariable("${ComponentPath1}|Components")
            };
        }
    }
}