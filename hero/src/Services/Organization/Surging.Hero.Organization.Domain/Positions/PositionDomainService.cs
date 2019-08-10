using System;
using System.Data.Common;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Domain.Positions
{
    public class PositionDomainService : IPositionDomainService
    {
        private readonly IDapperRepository<Position, long> _positionRepository;

        public PositionDomainService(IDapperRepository<Position, long> positionRepository) {
            _positionRepository = positionRepository;
        }

        public async Task CreatePosition(CreatePositionInput input, DbConnection conn, DbTransaction trans)
        {
            await CheckPosition(input);
            var position = input.MapTo<Position>();
            await _positionRepository.InsertAsync(position, conn, trans);
        }

        public async Task CreatePosition(CreatePositionInput input)
        {
            await CheckPosition(input);
            var position = input.MapTo<Position>();
            await _positionRepository.InsertAsync(position);
        }

        private async Task CheckPosition(CreatePositionInput input)
        {
            var position = await _positionRepository.SingleOrDefaultAsync(p => p.Code == input.Code);
            if (position != null) {
                throw new BusinessException($"系统中已经存在Code为{input.Code}的职位信息");
            }
            
        }
    }
}
