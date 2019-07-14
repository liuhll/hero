using Jose;
using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.CPlatform.Jwt.Implementation
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly ILogger<JwtTokenProvider> _logger;

        public JwtTokenProvider(ILogger<JwtTokenProvider> logger)
        {
            _logger = logger;
        }

        public string GenerateToken(IDictionary<string, object> headers, IDictionary<string, object> payload, string secretKey, JwsAlgorithm jwsAlgorithm = JwsAlgorithm.HS256)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new AuthException("未配置secretKey");
            }
            var secret = Encoding.UTF8.GetBytes(secretKey);
            string token = Jose.JWT.Encode(payload, secret, jwsAlgorithm, extraHeaders: headers);
            return token;
        }

        public IDictionary<string, object> GetPayLoad(string token, string secretKey, JwsAlgorithm jwsAlgorithm = JwsAlgorithm.HS256)
        {
            if (ValidateToken(token, secretKey, jwsAlgorithm))
            {
                var secret = Encoding.UTF8.GetBytes(secretKey);
                var payload = Jose.JWT.Decode<IDictionary<string, object>>(token, secret, jwsAlgorithm);
                return payload;
            }
            return null;
        }

        public bool ValidateToken(string token, string secretKey, JwsAlgorithm jwsAlgorithm = JwsAlgorithm.HS256)
        {
            try
            {
                var secret = Encoding.UTF8.GetBytes(secretKey);
                var payload = Jose.JWT.Decode<IDictionary<string, object>>(token, secret, jwsAlgorithm);
                var exp = Convert.ToDateTime(payload["exp"]);
                if (exp < DateTime.Now)
                {
                    throw new AuthException("登录超时,请重新登录");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("token验证失败,原因:" + ex.Message, ex);
                }
                return false;
            }

        }

    }
}
