using System;
using System.Collections.Generic;
using System.Text;
using Surging.Core.CPlatform.Messages;

namespace Surging.Core.CPlatform.Exceptions
{
    public class AuthException : CPlatformException
    {
        public AuthException(string message, StatusCode status = StatusCode.UnAuthentication) : base(message, status)
        {
        }

        public AuthException(string message, Exception innerException, StatusCode status = StatusCode.UnAuthentication) : base(message, innerException, status)
        {
        }
    }
}
