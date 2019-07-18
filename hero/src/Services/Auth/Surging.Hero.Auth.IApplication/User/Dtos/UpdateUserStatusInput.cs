using Surging.Hero.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class UpdateUserStatusInput
    {
        public long Id { get; set; }

        public Status Status { get; set; }
    }
}
