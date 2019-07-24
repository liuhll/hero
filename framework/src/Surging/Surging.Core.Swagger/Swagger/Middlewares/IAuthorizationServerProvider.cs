using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Core.Swagger.Middlewares
{
    public interface IAuthorizationServerProvider
    {

        Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters);
        IDictionary<string, object> GetPayload(string token);
    }
}
