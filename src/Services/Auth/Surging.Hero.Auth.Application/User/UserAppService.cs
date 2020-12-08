using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.IApplication.User;
using Surging.Hero.Auth.IApplication.User.Dtos;
using System.Threading.Tasks;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Core.AutoMapper;
using Surging.Core.Dapper.Repositories;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Domain.PagedAndSorted;
using System.Collections.Generic;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Position;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using System.Linq;
using System;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using System.Linq.Expressions;
using Surging.Core.CPlatform.Runtime.Session;

namespace Surging.Hero.Auth.Application.User
{
    [ModuleName(ModuleNameConstants.UserModule, Version = ModuleNameConstants.ModuleVersionV1)]
    public class UserAppService : ProxyServiceBase, IUserAppService
    {
        private readonly IUserDomainService _userDomainService;
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly ISurgingSession _session;
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
            var existUser = await _userRepository.FirstOrDefaultAsync(p => p.UserName == input.UserName);
            if (existUser != null)
            {
                throw new UserFriendlyException($"已经存在用户名为{input.UserName}的用户");
            }
            existUser = await _userRepository.FirstOrDefaultAsync(p => p.Phone == input.Phone);
            if (existUser != null)
            {
                throw new UserFriendlyException($"已经存在手机号码为{input.Phone}的用户");
            }
            existUser = await _userRepository.FirstOrDefaultAsync(p => p.Email == input.Email);
            if (existUser != null)
            {
                throw new UserFriendlyException($"已经存在Email为{input.Email}的用户");
            }

            await _userDomainService.Create(input);
            return "新增员工成功";
        }

        public async Task<string> Update(UpdateUserInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _userDomainService.Update(input);
            return "更新员工信息成功";
        }


        public async Task<string> Delete(long id)
        {
            if (_session.UserId.Value == id) 
            {
                throw new BusinessException($"不允许删除当前登录用户");
            }
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userInfo == null)
            {
                throw new BusinessException($"不存在Id为{id}的账号信息");
            }
            await _userDomainService.Delete(id);
            return "删除员工成功";
        }

        public async Task<IPagedResult<GetUserNormOutput>> Query(QueryUserInput query)
        {
           
            Expression<Func<UserInfo, bool>> expression = p => p.UserName.Contains(query.SearchKey)
                || p.ChineseName.Contains(query.SearchKey)
                || p.Email.Contains(query.SearchKey)
                || p.Phone.Contains(query.SearchKey);
           

           
            if (query.Status.HasValue) 
            {
                expression = expression.And(p => p.Status == query.Status.Value);
            }
            var queryResult = await _userRepository.GetAllAsync(expression);

            if (query.OrgId.HasValue && query.OrgId != 0)
            {
                var subOrgIds = await GetService<IOrganizationAppService>().GetSubOrgIds(query.OrgId.Value);
                queryResult = queryResult.Where(p => subOrgIds.Any(q=> q == p.OrgId));
            }

            var queryResultOutput = queryResult.MapTo<IEnumerable<GetUserNormOutput>>().GetPagedResult(queryResult.Count());
            foreach (var userOutput in queryResultOutput.Items)
            {
                if (userOutput.OrgId.HasValue)
                {
                    userOutput.DeptName = (await GetService<IDepartmentAppService>().GetByOrgId(userOutput.OrgId.Value)).Name;
                }
                if (userOutput.PositionId.HasValue)
                {
                    userOutput.PositionName = (await GetService<IPositionAppService>().Get(userOutput.PositionId.Value)).Name;
                }
                userOutput.Roles = (await _userDomainService.GetUserRoles(userOutput.Id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
                if (userOutput.LastModifierUserId.HasValue) 
                {
                    var modifyUserInfo = (await _userRepository.SingleOrDefaultAsync(p=>p.Id == userOutput.LastModifierUserId.Value));
                    if (modifyUserInfo != null) 
                    {
                        userOutput.LastModificationUserName = modifyUserInfo.ChineseName;
                    }
                   
                }
                if (userOutput.CreatorUserId.HasValue)
                {
                    var creatorUserInfo = (await _userRepository.SingleOrDefaultAsync(p => p.Id == userOutput.CreatorUserId.Value));
                    if (creatorUserInfo != null)
                    {
                        userOutput.CreatorUserName = creatorUserInfo.ChineseName;
                    }
                }

            }

            return queryResultOutput;
        }

        public async Task<string> UpdateStatus(UpdateUserStatusInput input)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userInfo == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的账号信息");
            }
            userInfo.Status = input.Status;
            await _userRepository.UpdateAsync(userInfo);
            var tips = "账号激活成功";
            if (input.Status == Status.Invalid)
            {
                tips = "账号冻结成功";
            }
            return tips;
        }

        public async Task<string> ResetPassword(ResetPasswordInput input)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userInfo == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的账号信息");
            }
            await _userDomainService.ResetPassword(userInfo, input.NewPassword);
            return "重置该员工密码成功";
        }

        public async Task<IEnumerable<GetUserBasicOutput>> GetOrgUser(long orgId, bool includeSubOrg)
        {
            IEnumerable<UserInfo> orgUsers = new List<UserInfo>();
            if (includeSubOrg)
            {
                var subOrdIds = await GetService<IOrganizationAppService>().GetSubOrgIds(orgId);
                orgUsers = (await _userRepository.GetAllAsync()).Where(p => subOrdIds.Any(q => q == p.OrgId));
            }
            else
            {
                orgUsers = await _userRepository.GetAllAsync(p => p.OrgId == orgId);
            }

            var orgUserOutputs = orgUsers.MapTo<IEnumerable<GetUserBasicOutput>>();
            foreach (var userOutput in orgUserOutputs)
            {
                if (userOutput.OrgId.HasValue)
                {
                    userOutput.DeptName = (await GetService<IDepartmentAppService>().GetByOrgId(userOutput.OrgId.Value)).Name;
                }
                if (userOutput.PositionId.HasValue)
                {
                    userOutput.PositionName = (await GetService<IPositionAppService>().Get(userOutput.PositionId.Value)).Name;
                }
            }
            return orgUserOutputs;
        }

        public async Task<GetUserNormOutput> Get(long id)
        {
            return await _userDomainService.GetUserNormInfoById(id);
        }

        //public async Task<IEnumerable<GetUserRoleOutput>> QueryUserRoles(QueryUserRoleInput query)
        //{
        //    var userRoleOutputs = new List<GetUserRoleOutput>();

        //    var roles = await _roleRepository.GetAllAsync();
        //    if (query.UserId.HasValue && query.UserId.Value != 0)
        //    {
        //        var userInfo = await _userDomainService.GetUserNormInfoById(query.UserId.Value);

        //        foreach (var role in roles)
        //        {
        //            userRoleOutputs.Add(new GetUserRoleOutput()
        //            {
        //                RoleId = role.Id,
        //                Name = role.Name,
        //                Checked = userInfo.Roles.Any(p => p.Id == role.Id) ? CheckStatus.Checked : CheckStatus.UnChecked
        //            });
        //        }
        //    }

        //    foreach (var role in roles)
        //    {
        //        userRoleOutputs.Add(new GetUserRoleOutput()
        //        {
        //            RoleId = role.Id,                  
        //            Name = role.Name,
        //            Checked = CheckStatus.UnChecked
        //        });
        //    }
        //    return userRoleOutputs;
        //}

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
    }
}
