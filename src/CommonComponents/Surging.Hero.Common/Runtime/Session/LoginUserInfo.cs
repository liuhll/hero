using System;
using System.Collections.Generic;
using Surging.Hero.Auth.Domain.Shared.Users;

namespace Surging.Hero.Common.Runtime.Session
{
    public class LoginUserInfo
    {
        public long Id { get; set; }

        public long DeptId { get; set; }

        public string DeptName { get; set; }

        public long PositionId { get; set; }

        public string PositionName { get; set; }

        public string UserName { get; set; }

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

        public Status Status { get; set; }

        public IEnumerable<GetDisplayRoleOutput> Roles { get; set; }
    }

    public class GetDisplayRoleOutput
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}