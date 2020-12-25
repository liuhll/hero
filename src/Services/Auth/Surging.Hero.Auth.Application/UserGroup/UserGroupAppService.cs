using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.FullAuditDtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Runtime.Session;

namespace Surging.Hero.Auth.Application.UserGroup
{
    public class UserGroupAppService : ProxyServiceBase, IUserGroupAppService
    {
        private readonly IUserGroupDomainService _userGroupDomainService;
        private readonly IDapperRepository<Domain.UserGroups.UserGroup, long> _userGroupRepository;
        private readonly IDapperRepository<UserInfo, long> _userInfoRepository;

        private readonly IDapperRepository<UserGroupDataPermissionOrgRelation, long>
            _userGroupDataPermissionOrgRelationRepository;
        private readonly ISurgingSession _session;

        public UserGroupAppService(IUserGroupDomainService userGroupDomainService,
            IDapperRepository<Domain.UserGroups.UserGroup, long> userGroupRepository,
            IRoleDomainService roleDomainService,
            IDapperRepository<UserInfo, long> userRepository, 
            IDapperRepository<UserGroupDataPermissionOrgRelation, long> userGroupDataPermissionOrgRelationRepository)
        {
            _userGroupDomainService = userGroupDomainService;
            _userGroupRepository = userGroupRepository;
            _userInfoRepository = userRepository;
            _userGroupDataPermissionOrgRelationRepository = userGroupDataPermissionOrgRelationRepository;
            _session = NullSurgingSession.Instance;
        }

        public async Task<string> AllocationUsers(AllocationUserIdsInput input)
        {
            return await _userGroupDomainService.AllocationUsers(input);
        }

        public async Task<string> Create(CreateUserGroupInput input)
        {
            _session.CheckLoginUserDataPermision(input.DataPermissionType,"您设置的用户组的数据权限大于您拥有数据权限,系统不允许该操作");
            input.CheckDataAnnotations().CheckValidResult();
            await _userGroupDomainService.Create(input);
            return "新增用户组成功";
        }

        public async Task<string> Delete(long id)
        {
            await _userGroupDomainService.Delete(id);
            return "删除用户组信息成功";
        }

        public async Task<string> DeleteUserGroupUser(DeleteUserGroupUserInput input)
        {
            await _userGroupDomainService.DeleteUserGroupUser(input);
            return "删除用户成功";
        }

        public async Task<GetUserEditGroupOutput> Get(long id)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userGroup == null) throw new UserFriendlyException($"不存在id为{id}的用户组信息");

            var userGroupOutput = userGroup.MapTo<GetUserEditGroupOutput>();
            userGroupOutput.RoleIds = (await _userGroupDomainService.GetUserGroupRoleOutputs(id)).Select(p => p.Id).ToArray();
            userGroupOutput.PermissionIds =
                (await _userGroupDomainService.GetUserGroupPermissions(id)).Select(p => p.Id).ToArray();
            userGroupOutput.DataPermissionOrgIds =
                (await _userGroupDataPermissionOrgRelationRepository.GetAllAsync(p => p.UserGroupId == id))
                .Select(p => p.Id).ToArray();
            return userGroupOutput;
        }

        public async Task<IPagedResult<GetUserGroupOutput>> Search(QueryUserGroupInput query)
        {
            /*Expression<Func<Domain.UserGroups.UserGroup, bool>> predicate = p => p.Name.Contains(query.SearchKey);
            if (query.Status.HasValue) predicate = predicate.And(p => p.Status == query.Status);
            
            var queryResult = await _userGroupRepository.GetPageAsync(predicate, query.PageIndex, query.PageCount);

            var outputs = queryResult.Item1.MapTo<IEnumerable<GetUserGroupOutput>>().GetPagedResult(queryResult.Item2);
            foreach (var output in outputs.Items)
            {
                await output.SetAuditInfo();
                output.Roles = await _userGroupDomainService.GetUserGroupRoleOutputs(output.Id);
                output.Permissions = (await _userGroupDomainService.GetUserGroupPermissions(output.Id))
                    .MapTo<IEnumerable<GetDisplayPermissionOutput>>();
                output.DataPermissionOrgs = await _userGroupDomainService.GetUserGroupDataPermissionOrgs(output.Id);
            }*/
                
            return await _userGroupDomainService.Search(query);
        }

        public async Task<IPagedResult<GetUserNormOutput>> SearchUserGroupUser(QueryUserGroupUserInput query)
        {
            if (query.UserGroupId <= 0) throw new BusinessException("必须指定合法的UserGroupId");
            if (!query.Sorting.IsNullOrEmpty() && !AuthConstant.UserGroupSortingFileds.Any(p => p == query.Sorting))
                throw new BusinessException("指定的排序字段无效");
            return await _userGroupDomainService.SearchUserGroupUser(query);
        }

        public async Task<string> Update(UpdateUserGroupInput input)
        {
            _session.CheckLoginUserDataPermision(input.DataPermissionType,"您设置的用户组的数据权限大于您拥有数据权限,系统不允许该操作");
            input.CheckDataAnnotations().CheckValidResult();
            await _userGroupDomainService.Update(input);
            return "更新用户组成功";
            ;
        }

        public async Task<string> UpdateStatus(UpdateUserGroupStatusInput input)
        {
            await _userGroupDomainService.UpdateStatus(input);
            if (input.Status == Status.Valid) return "激活用户组状态成功";
            return "冻结用户组状态成功";
        }
    }
}