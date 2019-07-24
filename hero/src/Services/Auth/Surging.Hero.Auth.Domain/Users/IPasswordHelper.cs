using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.Domain.Users
{
    public interface IPasswordHelper : ITransientDependency
    {
        string EncryptPassword(string userName, string plainPassword);
    }
}
