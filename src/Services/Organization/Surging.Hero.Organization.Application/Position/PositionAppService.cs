using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.Dapper.Repositories;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Organization.Domain.Positions;
using Surging.Hero.Organization.IApplication.Position;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Application.Position
{
    public class PositionAppService : ProxyServiceBase, IPositionAppService
    {
        private readonly IPositionDomainService _positionDomainService;
        private readonly IDapperRepository<Domain.Positions.Position, long> _positionRepository;

        public PositionAppService(IPositionDomainService positionDomainService,
            IDapperRepository<Domain.Positions.Position, long> positionRepository) {
            _positionDomainService = positionDomainService;
            _positionRepository = positionRepository;
        }

        public async Task<bool> Check(long positionId)
        {
            var position = await _positionRepository.SingleOrDefaultAsync(p => p.Id == positionId);
            if (position == null) {
                return false;
            }
            return true;
        }

        public async Task<string> Create(CreatePositionInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _positionDomainService.CreatePosition(input);
            return "新增职位信息成功";
        }

        public async Task<string> Delete(long id)
        {
            await _positionDomainService.DeletePosition(id);
            return "删除职位信息成功";
        }

        public async Task<GetPositionOutput> Get(long id)
        {
            var position = await _positionRepository.GetAsync(id);
            return position.MapTo<GetPositionOutput>();
        }

        public async Task<string> Update(UpdatePositionInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _positionDomainService.UpdatePosition(input);
            return "更新职位信息成功";
        }
    }
}
