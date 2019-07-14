
namespace Surging.Core.Domain.Entities
{
    public interface ISoftDelete
    {
        int IsDeleted { get; set; }
    }
}
