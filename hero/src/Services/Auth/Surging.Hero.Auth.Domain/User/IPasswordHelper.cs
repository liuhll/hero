using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.Domain.User
{
    public interface IPasswordHelper : ITransientDependency
    {
        string EncryptPassword(string userName, string plainPassword);
    }
}
