using Surging.Hero.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class UpdateRoleStatusInput
    {
        public long Id { get; set; }

        public Status Status { get; set; }
    }
}
