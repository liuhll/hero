using Surging.Hero.Common;
using Surging.Hero.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class GetRoleOutput : RoleDtoBase
    {
        public long Id { get; set; }

        public string DeptName { get; set; }

        public Status Status { get; set; }

        public string StatusDesc { get { return Status.GetDescription(); } }
    }
}
