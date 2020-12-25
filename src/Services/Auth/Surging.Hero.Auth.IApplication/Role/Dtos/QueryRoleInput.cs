﻿using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class QueryRoleInput : PagedAndSingleSortedResultRequest
    {

        public string SearchKey { get; set; }
        
        public bool OnlySelfOrgRole { get; set; } = true;

        public Status? Status { get; set; }
    }
}