using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class UpdateUserGroupStatusInput
    {
        public long Id { get; set; }

        public Status Status { get; set; }
    }
}
