using Surging.Core.CPlatform.Engines.Implementation;
using Surging.Core.CPlatform.Utilities;

namespace Surging.Test.Server
{
    public class SurgingServiceEngine: VirtualPathProviderServiceEngine
    {
        public SurgingServiceEngine()
        {
        
            ModuleServiceLocationFormats = new[] {
                EnvironmentHelper.GetEnvironmentVariable("${ModulePath1}|Modules"),
            };
            ComponentServiceLocationFormats  = new[] {
                 EnvironmentHelper.GetEnvironmentVariable("${ComponentPath1}|Components"),
            };
        }
    }
}
