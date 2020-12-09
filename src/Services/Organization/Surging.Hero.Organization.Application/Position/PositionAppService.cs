using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Repositories;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.IApplication.User;
using Surging.Hero.Organization.Domain.Positions;
using Surging.Hero.Organization.IApplication.Position;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Application.Position
{
    public class PositionAppService : ProxyServiceBase, IPositionAppService
    {
        private readonly IPositionDomainService _positionDomainService;
        private readonly IDapperRepository<Domain.Positions.Position, long> _positionRepository;
        private readonly IDapperRepository<Domain.Department,long> _departmentRepository;

        public PositionAppService(IPositionDomainService positionDomainService,
            IDapperRepository<Domain.Positions.Position, long> positionRepository,
            IDapperRepository<Domain.Department, long> departmentRepository) {
            _positionDomainService = positionDomainService;
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
        }




        public async Task<bool> CheckExsit(long positionId)
        {
            var position = await _positionRepository.SingleOrDefaultAsync(p => p.Id == positionId);
            if (position == null) {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckCanDeletePosition(long positionId)
        {
            var userAppServiceProxy = GetService<IUserAppService>();
            var positionUserCount = await userAppServiceProxy.GetPositionUserCount(positionId);
            if (positionUserCount >0) 
            {
                return false;
            }
            return true;
        }

        public async Task<GetPositionOutput> Get(long id)
        {
            var position = await _positionRepository.GetAsync(id);
            return position.MapTo<GetPositionOutput>();
        }


        public async Task<IEnumerable<GetPositionOutput>> GetDeptPosition(long deptId)
        {
            var positions = await _positionRepository.GetAllAsync(p=>p.DeptId == deptId);
            return positions.MapTo<IEnumerable<GetPositionOutput>>();
        }

        public async Task<IEnumerable<GetPositionOutput>> GetDeptPositionByOrgId(long orgId)
        {
            var dept = await _departmentRepository.SingleOrDefaultAsync(p => p.OrgId == orgId);
            if (dept == null) {
                throw new BusinessException($"不存在OrgId为{orgId}的部门信息");
            }
            var positions = await _positionRepository.GetAllAsync(p => p.DeptId == dept.Id);
            return positions.MapTo<IEnumerable<GetPositionOutput>>();
        }
     
    }
}
