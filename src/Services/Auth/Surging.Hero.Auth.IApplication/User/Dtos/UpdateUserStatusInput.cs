using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class UpdateUserStatusInput
    {
        public long Id { get; set; }

        public Status Status { get; set; }
    }
}
