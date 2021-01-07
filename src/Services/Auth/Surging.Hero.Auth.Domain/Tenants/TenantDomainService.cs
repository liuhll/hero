using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.Domain.PagedAndSorted.Extensions;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.FullAuditDtos;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Organization.Domain.Shared;
using Surging.Hero.Organization.IApplication.Corporation;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Auth.Domain.Tenants
{
    public class TenantDomainService : ManagerBase, ITenantDomainService
    {
        private readonly IDapperRepository<Tenant, long> _tenantRepository;
        private readonly IRoleDomainService _roleDomainService;
        private readonly IUserDomainService _userDomainService;
        private readonly IDapperRepository<Permission, long> _permissionRepository;
        private readonly ITenantConfigProvider _tenantConfigProvider;
        public TenantDomainService(IDapperRepository<Tenant, long> tenantRepository,
            IRoleDomainService roleDomainService,
            IUserDomainService userDomainService,
            IDapperRepository<Permission, long> permissionRepository, 
            ITenantConfigProvider tenantConfigProvider)
        {
            _tenantRepository = tenantRepository;
            _roleDomainService = roleDomainService;
            _userDomainService = userDomainService;
            _permissionRepository = permissionRepository;
            _tenantConfigProvider = tenantConfigProvider;
        }

        public async Task<string> Create(CreateTenantInput input)
        {
            var exsitTenant = await _tenantRepository.FirstOrDefaultAsync(p => p.Name == input.Name.Trim());
            if (exsitTenant != null)
            {
                throw new BusinessException($"已经存在{input.Name}的租户");
            }

            var corporationAppServiceProxy = GetService<ICorporationAppService>();
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                var tenantId = await _tenantRepository.InsertAndGetIdAsync(input.MapTo<Tenant>(), conn, trans);
                if (input.CreateSuper)
                {
                    await CreateAdminAndRole(tenantId, input, conn, trans);
                }

                await corporationAppServiceProxy.CreateByTenant(new CreateCorporationByTenantInput()
                {
                    Name = input.Name,
                    Identification = input.Identification,
                    ParentId = 0,
                    OpenDate = DateTime.Now,
                    RegisterDate = DateTime.Now,
                    Mold = CorporationMold.Group,
                    TenantId = tenantId
                });
            }, Connection);

            return "新增租户成功";
        }

        private async Task CreateAdminAndRole(long tenantId, CreateTenantInput input, DbConnection conn,
            DbTransaction trans)
        {
            var tenantConfig = _tenantConfigProvider.Get();
            if (tenantConfig == null || tenantConfig.SuperUserPassword.IsNullOrWhiteSpace() ||
                tenantConfig.SuperUserPassword.IsNullOrWhiteSpace())
            {
                throw new BusinessException("获取租户默认管理员账号和密码失败");
            }

            var permissions = await _permissionRepository.GetAllAsync();
            var createRole = new CreateRoleInput()
            {
                Identification = input.Identification + "_admin",
                Name = "管理员",
                PermissionIds = permissions.Select(p => p.Id).ToArray(),
                DataPermissionType = DataPermissionType.AllOrg,
                IsAllOrg = true,
                Memo = "创建租户时,初始化的角色",
            };
            var roleId = await _roleDomainService.Create(createRole, conn, trans, tenantId);
            var createUser = new CreateUserInput()
            {
                OrgId = null,
                UserName = tenantConfig.SuperUserAccount,
                Password = tenantConfig.SuperUserPassword,
                ChineseName = tenantConfig.ChineseName?? "管理员",
                Memo = "创建租户时,初始化的用户",
                RoleIds = new[] {roleId},
                Status = Common.Status.Valid
            };
            await _userDomainService.Create(createUser, conn, trans, tenantId);
        }

        public async Task<string> Update(UpdateTenantInput input)
        {
            var tenant = await _tenantRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (tenant == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的租户信息");
            }

            if (!input.Name.Equals(tenant.Name))
            {
                var exsitTenant = await _tenantRepository.FirstOrDefaultAsync(p => p.Name == input.Name.Trim());
                if (exsitTenant != null)
                {
                    throw new BusinessException($"已经存在{input.Name}的租户");
                }
            }

            tenant = input.MapTo(tenant);
            await _tenantRepository.UpdateAsync(tenant);
            return "更新租户信息成功";
        }

        public async Task<string> Delete(long id)
        {
            var tenant = await _tenantRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (tenant == null)
            {
                throw new BusinessException($"不存在Id为{id}的租户信息");
            }

            await _tenantRepository.DeleteAsync(p => p.Id == id);
            return "租户删除成功";
        }

        public async Task<IPagedResult<GetTenantPageOutput>> Search(QueryTenantInput query)
        {
            var sort = new Dictionary<string, SortType>();
            if (!query.Sorting.IsNullOrEmpty())
            {
                sort.Add(query.Sorting, query.SortType);
            }
            else
            {
                sort.Add("Id", SortType.Desc);
            }

            var queryResult = await _tenantRepository.GetPageAsync(p => p.Name.Contains(query.Name), query.PageIndex,
                query.PageCount, sort);

            var output = queryResult.Item1.MapTo<IEnumerable<GetTenantPageOutput>>().GetPagedResult(queryResult.Item2);
            foreach (var item in output.Items)
            {
                await item.SetAuditInfo();
            }

            return output;
        }

        public async Task<string> Status(UpdateTenantStatusInput input)
        {
            var tenant = await _tenantRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (tenant == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的租户信息");
            }

            tenant.Status = input.Status;
            await _tenantRepository.UpdateAsync(tenant);
            if (input.Status == Common.Status.Valid) return "启用租户成功";
            return "禁用租户成功";
        }

        public async Task<IEnumerable<GetTenantOutput>> List()
        {
            return (await _tenantRepository.GetAllAsync(p => p.Status == Common.Status.Valid))
                .MapTo<IEnumerable<GetTenantOutput>>();
        }
    }
}