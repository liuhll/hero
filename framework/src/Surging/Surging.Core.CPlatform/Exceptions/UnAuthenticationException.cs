using Surging.Core.CPlatform.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.CPlatform.Exceptions
{
    public class UnAuthenticationException : AuthException
    {
        public UnAuthenticationException(string message, Exception innerException = null, StatusCode status = StatusCode.UnAuthentication) : base(message, innerException, status)
        {
        }
    }
}
