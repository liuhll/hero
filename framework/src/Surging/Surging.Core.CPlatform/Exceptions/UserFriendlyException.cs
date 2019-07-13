using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public class UserFriendlyException : CPlatformException
    {
        public UserFriendlyException(string message, Exception innerException = null) : base(message, innerException, StatusCode.ValidateError)
        {
        }

        public UserFriendlyException(string message) : base(message, StatusCode.UserFriendly)
        {

        }
    }
}
