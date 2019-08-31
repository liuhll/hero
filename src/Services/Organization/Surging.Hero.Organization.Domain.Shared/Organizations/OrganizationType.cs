using System.ComponentModel;

namespace Surging.Hero.Organization.Domain.Shared.Organizations
{
    public enum OrganizationType
    {
        [Description("母公司")]
        ParentFirm = 0,

        [Description("子公司")]
        Subsidiary,

        [Description("股份制公司")]
        HoldingCompany,

        [Description("部门")]
        Department
    }
}
