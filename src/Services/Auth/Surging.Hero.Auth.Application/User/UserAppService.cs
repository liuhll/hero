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
using Surging.Core.Domain;
using System.Collections.Generic;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Position;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Application.User
{
    [ModuleName(ModuleNameConstants.UserModule, Version = ModuleNameConstants.ModuleVersionV1)]
    public class UserAppService : ProxyServiceBase, IUserAppService
    {
        private readonly IUserDomainService _userDomainService;
        private readonly IDapperRepository<UserInfo, long> _userRepository;

        public UserAppService(IUserDomainService userDomainService,
            IDapperRepository<UserInfo, long> userRepository)
        {
            _userDomainService = userDomainService;
            _userRepository = userRepository;
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
            var queryResult = await _userRepository.GetPageAsync(p => p.UserName.Contains(query.SearchKey)
               || p.ChineseName.Contains(query.SearchKey)
               || p.Email.Contains(query.SearchKey)
               || p.Phone.Contains(query.SearchKey),query.PageIndex,query.PageCount); 
            
            var queryResultOutput = queryResult.Item1.MapTo<IEnumerable<GetUserNormOutput>>().GetPagedResult(queryResult.Item2);
            foreach (var userOutput in queryResultOutput.Items) {
                userOutput.DeptName = (await GetService<IDepartmentAppService>().Get(userOutput.DeptId)).Name;
                userOutput.PositionName = (await GetService<IPositionAppService>().Get(userOutput.PositionId)).Name;
                userOutput.Roles = (await _userDomainService.GetUserRoles(userOutput.Id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
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

        public async Task<IEnumerable<GetUserBasicOutput>> GetDepartmentUser(long deptId)
        {
            var departUsers = await _userRepository.GetAllAsync(p => p.DeptId == deptId);
            var departUserOutputs = departUsers.MapTo<IEnumerable<GetUserBasicOutput>>();
            foreach (var userOutput in departUserOutputs)
            {
                userOutput.DeptName = (await GetService<IDepartmentAppService>().Get(userOutput.DeptId)).Name;
                userOutput.PositionName = (await GetService<IPositionAppService>().Get(userOutput.PositionId)).Name;
            }
            return departUserOutputs;
        }

        public async Task<IEnumerable<GetUserBasicOutput>> GetCorporationUser(long corporationId)
        {
            var corporationUsers = await _userRepository.GetAllAsync(p => p.DeptId == corporationId);           
            var corporationUserOutputs = corporationUsers.MapTo<IEnumerable<GetUserBasicOutput>>();

            foreach (var userOutput in corporationUserOutputs)
            {
                userOutput.DeptName = (await GetService<IDepartmentAppService>().Get(userOutput.DeptId)).Name;
                userOutput.PositionName = (await GetService<IPositionAppService>().Get(userOutput.PositionId)).Name;
            }
            return corporationUserOutputs;
        }

        public async Task<GetUserNormOutput> Get(long id)
        {
            var userInfo = await _userRepository.GetAsync(id);
            var userInfoOutput = userInfo.MapTo<GetUserNormOutput>();
            userInfoOutput.DeptName = (await GetService<IDepartmentAppService>().Get(userInfoOutput.DeptId)).Name;
            userInfoOutput.PositionName = (await GetService<IPositionAppService>().Get(userInfoOutput.PositionId)).Name;
            userInfoOutput.Roles = (await _userDomainService.GetUserRoles(id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
            return userInfoOutput;
        }
    }
}
