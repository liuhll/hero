namespace Surging.Core.Domain.Entities.Auditing
{
    public interface IFullAudited : IAudited, IDeletionAudited
    {
    }

    public interface IFullAudited<TUser> : IAudited<TUser>, IFullAudited, IDeletionAudited<TUser>
        where TUser : IEntity<long>
    {
    }
}
