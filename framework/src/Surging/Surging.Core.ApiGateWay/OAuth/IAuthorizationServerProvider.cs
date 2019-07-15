using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Core.ApiGateWay.OAuth
{
    public interface IAuthorizationServerProvider
    {

        Task<string> GenerateTokenCredential(IDictionary<string, object> rpcParams);

        Task<bool> ValidateClientAuthentication(string token);

        Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters);

        IDictionary<string, object> GetPayLoad(string token);
    }
}