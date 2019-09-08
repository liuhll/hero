using Surging.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class QueryUserInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }

    }
}
