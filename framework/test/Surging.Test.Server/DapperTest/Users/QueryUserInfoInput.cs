using Surging.Core.Domain.PagedAndSorted;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Test.Server.DapperTest.Users
{
    public class QueryUserInfoInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }
    }
}
