namespace Surging.Core.Domain.Entities.Auditing
{
    public interface ICreationAudited : IHasCreationTime
    {
        long? CreatorUserId { get; set; }
    }

    public interface ICreationAudited<TUser> : ICreationAudited
        where TUser : IEntity<long>
    {
        TUser CreatorUser { get; set; }
    }
}
