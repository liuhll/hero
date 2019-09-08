using System.Collections.Generic;

namespace Surging.Core.Domain
{
    public interface ITree<T> where T : class
    {
        long Id { get; set; }
        string Code { get; set; }

        string Name { get; set; }

        long ParentId { get; set; }

        string FullName { get; set; }

        IEnumerable<ITree<T>> Children { get; set; }

    }
}
