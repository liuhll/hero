using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupDomainService : ManagerBase, IUserGroupDomainService
    {
        private readonly IDapperRepository<UserGroup, long> _userGroupRepository;
        private readonly IDapperRepository<UserGroupRole, long> _userGroupRoleRepository;
        private readonly IDapperRepository<UserUserGroupRelation, long> _userUserGroupRelationRepository;

        public UserGroupDomainService(IDapperRepository<UserGroup, long> userGroupRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRepository) {
            _userGroupRepository = userGroupRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _userUserGroupRelationRepository = userUserGroupRelationRepository;
        }

        public async Task Create(CreateUserGroupInput input)
        {
            var userGroup = input.MapTo<UserGroup>();
            var thisLevelUserGroupCount = await _userGroupRepository.GetCountAsync(p => p.ParentId == input.ParentId);
            if (input.ParentId == 0)
            {
                userGroup.Level = 1;
                userGroup.Code = (thisLevelUserGroupCount + 1).ToString().PadRight(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            }
            else {
                var parentUserGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.ParentId);
                if (parentUserGroup == null) {
                    throw new BusinessException($"不存在Id为{input.ParentId}的用户组信息");
                }
                userGroup.Level = parentUserGroup.Level + 1;
                userGroup.Code = parentUserGroup.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (thisLevelUserGroupCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            }
            await _userGroupRepository.InsertAsync(userGroup);
        }

        public async Task Delete(long id)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userGroup == null) {
                throw new BusinessException($"不存在Id为{id}的用户组信息");
            }
            var children = await _userGroupRepository.GetAllAsync(p => p.ParentId == id);
            if (children.Any()) {
                throw new BusinessException("请先删除子用户组");
            }
            await UnitOfWorkAsync(async(conn,trans) => {
                await _userGroupRepository.DeleteAsync(p => p.Id == id,conn,trans);
                await _userGroupRoleRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);
                await _userUserGroupRelationRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);
            }, Connection);
        }

        public async Task Update(UpdateUserGroupInput input)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userGroup == null) {
                throw new BusinessException($"不存在Id为{input.Id}的用户组");
            }
            userGroup = input.MapTo(userGroup);
            await _userGroupRepository.UpdateAsync(userGroup);
        }
    }
}
