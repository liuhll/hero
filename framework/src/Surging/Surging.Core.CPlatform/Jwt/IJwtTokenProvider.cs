using Jose;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.CPlatform.Jwt
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(IDictionary<string, object> header, IDictionary<string, object> payload, string secretKey, JwsAlgorithm jwsAlgorithm = JwsAlgorithm.HS256);

        IDictionary<string, object> GetPayLoad(string token, string secretKey, JwsAlgorithm jwsAlgorithm = JwsAlgorithm.HS256);

        bool ValidateToken(string token, string secretKey, JwsAlgorithm jwsAlgorithm = JwsAlgorithm.HS256);
    }
}
