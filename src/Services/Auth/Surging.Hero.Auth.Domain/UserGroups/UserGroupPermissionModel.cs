namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupPermissionModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public long OperationId { get; set; }
    }
}