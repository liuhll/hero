namespace Surging.Core.Domain.Entities.Auditing
{
    public interface IDeletionAudited : IHasDeletionTime
    {
        long? DeleterUserId { get; set; }
    }

    public interface IDeletionAudited<TUser> : IDeletionAudited
        where TUser : IEntity<long>
    {
        TUser DeleterUser { get; set; }
    }
}
