using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class GetUserRoleOutput
    {
        public long RoleId { get; set; }

        public string Name { get; set; }

        public CheckStatus Checked { get; set; }
    }
}