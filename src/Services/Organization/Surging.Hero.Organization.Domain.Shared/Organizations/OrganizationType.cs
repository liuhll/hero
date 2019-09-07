using System.ComponentModel;

namespace Surging.Hero.Organization.Domain.Shared.Organizations
{
    public enum OrganizationType
    {
        Corporation = 0,

        [Description("部门")]
        Department
    }
}
