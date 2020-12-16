using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class UpdateRoleStatusInput
    {
        public long Id { get; set; }

        public Status Status { get; set; }
    }
}