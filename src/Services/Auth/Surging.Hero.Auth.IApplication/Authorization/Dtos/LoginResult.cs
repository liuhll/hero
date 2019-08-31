using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.Authorization.Dtos
{
    public class LoginResult
    {
        public LoginResultType ResultType { get; set; }

        public string ErrorMessage { get; set; }

        public IDictionary<string, object> PayLoad { get; set; }
    }
}
