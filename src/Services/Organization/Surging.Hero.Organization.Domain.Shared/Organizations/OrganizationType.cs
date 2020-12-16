using System.ComponentModel;

namespace Surging.Hero.Organization.Domain.Shared.Organizations
{
    public enum OrganizationType
    {
        [Description("公司")] Corporation = 0,

        [Description("部门")] Department
    }
}