using System.ComponentModel;

namespace Surging.Hero.Auth.Domain.Shared.Roles
{
    public enum RoleType
    {
        [Description("通用")] Universal = 0,

        [Description("部门")] Department
    }
}