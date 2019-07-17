using Surging.Hero.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.Domain.User
{
    public class PasswordHelper : IPasswordHelper
    {
        public string EncryptPassword(string userName, string plainPassword)
        {
            return EncryptHelper.Md5(EncryptHelper.Md5(userName + plainPassword));
        }
    }
}
