namespace Surging.Hero.Auth.Domain
{
    public static class AuthDomainConstants
    {
        public static class ClaimTypes
        {
            public const string OrgId = "http://www.liuhl-hero.top/claimtypes/orgid";
        }

        public static string[] PermissionServiceIdWhiteList = new[] { "Surging.Hero.Organization.IApplication.Organization.IOrganizationAppService.GetOwnTree" };
    }
}