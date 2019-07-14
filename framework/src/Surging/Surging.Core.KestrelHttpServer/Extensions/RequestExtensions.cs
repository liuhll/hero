using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace Surging.Core.KestrelHttpServer.Extensions
{
    public static class RequestExtensions
    {
        public static string GetTokenFromHeader(this HttpRequest request)
        {
            var token = request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
            {
                throw new AuthenticationException("不存在Token凭证");
            }

            return token;
        }
    }
}
