using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Domain.Positions
{
    public interface IPositionDomainService : ITransientDependency
    {
        Task CreatePosition(Position input, string positionCode, DbConnection conn, DbTransaction trans);

        Task<IEnumerable<GetPositionOutput>> GetPositionsByDeptId(long deptId);
        Task UpdatePosition(UpdatePositionInput input);
        Task DeletePosition(long id);
    }
}