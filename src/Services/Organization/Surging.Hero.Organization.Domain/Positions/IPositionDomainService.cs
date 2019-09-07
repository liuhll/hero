using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Domain.Positions
{
    public interface IPositionDomainService : ITransientDependency
    {
        Task CreatePosition(CreatePositionInput input,string positionCode, DbConnection conn, DbTransaction trans);

        Task CreatePosition(CreatePositionInput input);
        Task<IEnumerable<Position>> GetPositionsByDeptId(long deptId);
        Task UpdatePosition(UpdatePositionInput input);
        Task DeletePosition(long id);
    }
}
