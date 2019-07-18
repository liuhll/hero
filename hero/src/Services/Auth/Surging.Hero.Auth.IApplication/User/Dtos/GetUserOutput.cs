using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class GetUserOutput : UserDtoBase
    {
        public long Id { get; set; }
        public string UserName { get; set; }
    }
}
