using Surging.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Test.Server.DapperTest.Users
{
    public class UserInfo : FullAuditedEntity<long>
    {
        public UserInfo()
        {
            LoginFailedCount = 0;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ChineseName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int Gender { get; set; }

        public DateTime Birth { get; set; }

        public string NativePlace { get; set; }

        public string Address { get; set; }

        public string Folk { get; set; }

        public int PoliticalStatus { get; set; }

        public string GraduateInstitutions { get; set; }

        public string Education { get; set; }

        public string Major { get; set; }

        public string Resume { get; set; }

        public string Memo { get; set; }

        public DateTime LastLoginTime { get; set; }

        public int LoginFailedCount { get; set; }

        public int Status { get; set; }

        public long DeptId { get; set; }

    }
}
