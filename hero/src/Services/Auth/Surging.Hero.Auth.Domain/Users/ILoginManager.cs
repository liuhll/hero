using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Users
{
    public interface ILoginManager : ITransientDependency
    {
        Task<IDictionary<string, object>> Login(string userName, string password);
    }
}
