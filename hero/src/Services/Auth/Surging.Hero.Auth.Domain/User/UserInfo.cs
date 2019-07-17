using Surging.Core.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared.User;
using Surging.Hero.Common.Enums;
using System;

namespace Surging.Hero.Auth.Domain.User
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

        public Gender Gender { get; set; }

        public DateTime Birth { get; set; }

        public string NativePlace { get; set; }

        public string Address { get; set; }

        public string Folk { get; set; }

        public PoliticalStatus PoliticalStatus { get; set; }

        public string GraduateInstitutions { get; set; }

        public string Education { get; set; }

        public string Major { get; set; }

        public string Resume { get; set; }

        public string Memo { get; set; }

        public DateTime LastLoginTime { get; set; } 

        public int LoginFailedCount { get; set; }

        public Status Status { get; set; }

    }
}


