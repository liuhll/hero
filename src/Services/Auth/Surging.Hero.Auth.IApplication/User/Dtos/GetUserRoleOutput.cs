using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class GetUserRoleOutput
    {
        public long RoleId { get; set; }

        public string Name { get; set; }

        public long? DeptId { get; set; }

        public string DeptName { get; set; }

        public CheckStatus CheckStatus { get; set; }

        public string DisplayName {
            get {
                if (DeptId.HasValue && DeptId != 0) {
                    return $"{Name}({DeptName})";
                }
                return Name;
            }
        }
    }
}
