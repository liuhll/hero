using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class ResetPasswordInput
    {
        public long Id { get; set; }

        public string NewPassword { get; set; }

    }
}
