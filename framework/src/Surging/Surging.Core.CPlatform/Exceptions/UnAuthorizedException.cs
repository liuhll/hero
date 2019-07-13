using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public class UnAuthorizedException : AuthException
    {
        public UnAuthorizedException(string message, Exception innerException = null, StatusCode status = StatusCode.UnAuthorized) : base(message, innerException, status)
        {
        }
    }
}
