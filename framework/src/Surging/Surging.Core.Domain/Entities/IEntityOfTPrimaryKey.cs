

namespace Surging.Core.Domain.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键Id,唯一标识
        /// </summary>
        TPrimaryKey Id { get; set; }

        bool IsTransient();
    }
}
