using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.IApplication.UserGroup;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;

namespace Surging.Hero.Auth.Application.UserGroup
{
    public class UserGroupAppService : ProxyServiceBase, IUserGroupAppService
    {
        private readonly IUserGroupDomainService _userGroupDomainService;
        private readonly IDapperRepository<Domain.UserGroups.UserGroup, long> _userGroupRepository;

        public UserGroupAppService(IUserGroupDomainService userGroupDomainService,
            IDapperRepository<Domain.UserGroups.UserGroup, long> userGroupRepository) {
            _userGroupDomainService = userGroupDomainService;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<string> Create(CreateUserGroupInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _userGroupDomainService.Create(input);
            return "新增用户组成功";
        }

        public async Task<string> Delete(long id)
        {
            await _userGroupDomainService.Delete(id);
            return "删除用户组信息成功";
        }

        public async Task<GetUserGroupOutput> Get(long id)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userGroup == null) {
                throw new UserFriendlyException($"不存在id为{id}的用户组信息");
            }
            return userGroup.MapTo<GetUserGroupOutput>();
        }

        public async Task<IEnumerable<ITree<GetUserGroupTreeOutput>>> GetTree()
        {
            var userGroups = await _userGroupRepository.GetAllAsync();
            var userGroupTreeOutputs = userGroups.MapTo<IEnumerable<GetUserGroupTreeOutput>>();
            return userGroupTreeOutputs.BuildTree();
        }

        public async Task<string> Update(UpdateUserGroupInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _userGroupDomainService.Update(input);
            return "更新用户组成功"; ;
        }

    }
}
