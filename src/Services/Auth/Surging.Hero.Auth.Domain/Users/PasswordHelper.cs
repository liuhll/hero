using Surging.Hero.Common.Utils;

namespace Surging.Hero.Auth.Domain.Users
{
    public class PasswordHelper : IPasswordHelper
    {
        public string EncryptPassword(string userName, string plainPassword)
        {
            return EncryptHelper.Md5(EncryptHelper.Md5(userName + plainPassword));
        }
    }
}