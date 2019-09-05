using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class GetRoleOutput : RoleDtoBase
    {
        public long Id { get; set; }

        public string DeptName { get; set; }
    }
}
