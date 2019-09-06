using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupDomainService : IUserGroupDomainService
    {
        private readonly IDapperRepository<UserGroup, long> _userGroupRepository;

        public UserGroupDomainService(IDapperRepository<UserGroup, long> userGroupRepository) {
            _userGroupRepository = userGroupRepository;
        }

        public async Task Create(CreateUserGroupInput input)
        {
            var userGroup = input.MapTo<UserGroup>();
            var thisLevelUserGroupCount = await _userGroupRepository.GetCountAsync(p => p.ParentId == input.ParentId);
            if (input.ParentId == 0)
            {
                userGroup.Level = 1;
                userGroup.Code = (thisLevelUserGroupCount + 1).ToString().PadRight(HeroConstants.CodeRuleRestrain.CodeCoverBit);
            }
            else {
                var parentUserGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.ParentId);
                if (parentUserGroup == null) {
                    throw new BusinessException($"不存在Id为{input.ParentId}的用户组信息");
                }
                userGroup.Level = parentUserGroup.Level + 1;
                userGroup.Code = parentUserGroup.Code + HeroConstants.CodeRuleRestrain.CodeCoverSymbol + (thisLevelUserGroupCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit);
            }
            await _userGroupRepository.InsertAsync(userGroup);
        }
    }
}
