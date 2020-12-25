using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Session;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Dapper.Repositories;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.ProxyGenerator;
using Surging.Cloud.System.Intercept;
using Surging.Cloud.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.User;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Runtime.Session;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Hero.Organization.IApplication.Position;

namespace Surging.Hero.Auth.Application.User
{
    [ModuleName(ModuleNameConstants.UserModule, Version = ModuleNameConstants.ModuleVersionV1)]
    public class UserAppService : ProxyServiceBase, IUserAppService
    {
        private readonly ISurgingSession _session;
        private readonly IUserDomainService _userDomainService;
        private readonly IDapperRepository<UserInfo, long> _userRepository;

        public UserAppService(IUserDomainService userDomainService,
            IDapperRepository<UserInfo, long> userRepository)
        {
            _userDomainService = userDomainService;
            _userRepository = userRepository;
            _session = NullSurgingSession.Instance;
        }


        public async Task<string> Create(CreateUserInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            _session.CheckLoginUserDataPermision(input.OrgId,"您没有新增该部门用户的权限");
            var existUser = await _userRepository.FirstOrDefaultAsync(p => p.UserName == input.UserName,false);
            if (existUser != null) throw new UserFriendlyException($"已经存在用户名为{input.UserName}的用户");
            existUser = await _userRepository.FirstOrDefaultAsync(p => p.Phone == input.Phone,false);
            if (existUser != null) throw new UserFriendlyException($"已经存在手机号码为{input.Phone}的用户");
            existUser = await _userRepository.FirstOrDefaultAsync(p => p.Email == input.Email,false);
            if (existUser != null) throw new UserFriendlyException($"已经存在Email为{input.Email}的用户");

            await _userDomainService.Create(input);
            return "新增员工成功";
        }

        public async Task<string> Update(UpdateUserInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            _session.CheckLoginUserDataPermision(input.OrgId,"您没有将用户设置为该部门的权限");
            await _userDomainService.Update(input);
            return "更新员工信息成功";
        }


        public async Task<string> Delete(long id)
        {
            if (_session.UserId.Value == id) throw new BusinessException("不允许删除当前登录用户");
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userInfo == null) throw new BusinessException($"不存在Id为{id}的账号信息");
            _session.CheckLoginUserDataPermision(userInfo.OrgId,"您没有删除该用户的权限");
            await _userDomainService.Delete(id);
            return "删除员工成功";
        }


        public async Task<IPagedResult<GetUserNormOutput>> Search(QueryUserInput query)
        {
            if (!query.Sorting.IsNullOrEmpty() && !AuthConstant.UserSortingFileds.Any(p => p == query.Sorting))
                throw new BusinessException("指定的排序字段无效");
            return await _userDomainService.Search(query);
        }

        public async Task<string> UpdateStatus(UpdateUserStatusInput input)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userInfo == null) throw new BusinessException($"不存在Id为{input.Id}的账号信息");
            userInfo.Status = input.Status;
            await _userRepository.UpdateAsync(userInfo);
            var tips = "账号激活成功";
            if (input.Status == Status.Invalid) tips = "账号冻结成功";
            return tips;
        }

        public async Task<string> ResetPassword(ResetPasswordInput input)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userInfo == null) throw new BusinessException($"不存在Id为{input.Id}的账号信息");
            await _userDomainService.ResetPassword(userInfo, input.NewPassword);
            return "重置该员工密码成功";
        }

        public async Task<IEnumerable<GetUserBasicOutput>> GetOrgUser(long orgId, bool includeSubOrg)
        {
            IEnumerable<UserInfo> orgUsers = new List<UserInfo>();
            if (includeSubOrg)
            {
                var subOrdIds = await GetService<IOrganizationAppService>().GetSubOrgIds(orgId);
                Expression<Func<UserInfo, bool>> expression = null;
                foreach (var subOrdId in subOrdIds)
                {
                    if (expression == null)
                    {
                        expression = p => p.OrgId == subOrdId;
                    }
                    else
                    {
                        expression = expression.Or(p => p.OrgId == subOrdId);
                    }
                }

                orgUsers = await _userRepository.GetAllAsync(expression);
            }
            else
            {
                orgUsers = await _userRepository.GetAllAsync(p => p.OrgId == orgId);
            }

            var orgUserOutputs = orgUsers.MapTo<IEnumerable<GetUserBasicOutput>>();
            foreach (var userOutput in orgUserOutputs)
            {
                if (userOutput.OrgId.HasValue)
                    userOutput.DeptName = (await GetService<IDepartmentAppService>().GetByOrgId(userOutput.OrgId.Value))
                        .Name;
                if (userOutput.PositionId.HasValue)
                    userOutput.PositionName =
                        (await GetService<IPositionAppService>().Get(userOutput.PositionId.Value)).Name;
            }

            return orgUserOutputs;
        }

        public async Task<int> GetOrgUserCount(long orgId, bool includeSubOrg)
        {
            var orgUserCount = 0;
            if (includeSubOrg)
            {
                var subOrdIds = await GetService<IOrganizationAppService>().GetSubOrgIds(orgId);
                Expression<Func<UserInfo, bool>> expression = null;
                foreach (var subOrdId in subOrdIds)
                {
                    if (expression == null)
                    {
                        expression = p => p.OrgId == subOrdId;
                    }
                    else
                    {
                        expression = expression.Or(p => p.OrgId == subOrdId);
                    }
                }

                orgUserCount = await _userRepository.GetCountAsync(expression);
            }
            else
            {
                orgUserCount = await _userRepository.GetCountAsync(p => p.OrgId == orgId);
            }

            return orgUserCount;
        }

        public async Task<GetUserNormOutput> Get(long id)
        {
            return await _userDomainService.GetUserNormInfoById(id);
        }
        

        public async Task<bool> ResetUserOrgInfo(long id)
        {
            var userInfo = await _userRepository.GetAsync(id);
            userInfo.OrgId = null;
            userInfo.PositionId = null;
            await _userRepository.UpdateAsync(userInfo);
            return true;
        }

        public async Task<int> GetPositionUserCount(long positionId)
        {
            return await _userRepository.GetCountAsync(p => p.PositionId == positionId);
        }

        public async Task<GetUserBasicOutput> GetUserBasicInfo([CacheKey(1)] long id)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == id,false);
            if (userInfo == null) return null;
            return userInfo.MapTo<GetUserBasicOutput>();
        }

        public async Task<bool> CheckCreateUser(long orgId)
        {
            return await Task.Run(() => _session.IsAllOrg || _session.DataPermissionOrgIds.Contains(orgId));
        }
    }
}