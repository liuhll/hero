﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.IApplication.Role;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Organization.IApplication.Department;

namespace Surging.Hero.Auth.Application.Role
{
    public class RoleAppService : ProxyServiceBase, IRoleAppService
    {
        private readonly IRoleDomainService _roleDomainService;
        private readonly IDapperRepository<Domain.Roles.Role, long> _roleRepository;
        private readonly IPermissionDomainService _permissionDomainService;

        public RoleAppService(IRoleDomainService roleDomainService,
            IDapperRepository<Domain.Roles.Role, long> roleRepository,
            IPermissionDomainService permissionDomainService) {
            _roleDomainService = roleDomainService;
            _roleRepository = roleRepository;
            _permissionDomainService = permissionDomainService;
        }

        public async Task<string> Create(CreateRoleInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _roleDomainService.Create(input);
            return "新增角色信息成功";
        }

        public async Task<string> Delete(long id)
        {
            await _roleDomainService.Delete(id);
            return "删除角色信息成功";
        }

        public async Task<GetRoleOutput> Get(long id)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (role == null) {
                throw new BusinessException($"不存在Id为{id}的角色信息");
            }
            var roleOutput = role.MapTo<GetRoleOutput>();
           
            return roleOutput;
     
        }

        public async Task<IEnumerable<GetRolePermissionTreeOutput>> GetRolePermissions(long id)
        {
            return await _permissionDomainService.GetRolePermissions(id);
        }

        public async Task<IEnumerable<GetRoleOutput>> List(string searchKey)
        {
            var queryRoles = await _roleRepository.GetAllAsync(p => p.Name.Contains(searchKey));
            return queryRoles.MapTo<IEnumerable<GetRoleOutput>>();
        }

        public async Task<IPagedResult<GetRoleOutput>> Query(QueryRoleInput query)
        {
            Tuple<IEnumerable<Domain.Roles.Role>, int> queryResult = queryResult = await _roleRepository.GetPageAsync(p => p.Name.Contains(query.SearchKey) && p.Memo.Contains(query.SearchKey), query.PageIndex, query.PageCount);

            var outputs = queryResult.Item1.MapTo<IEnumerable<GetRoleOutput>>().GetPagedResult(queryResult.Item2);
            
            return outputs;
        }

        public async Task<string> SetPermissions(SetRolePermissionInput input)
        {
            await _roleDomainService.SetPermissions(input);
            return "设置角色权限信息成功";
        }

        public async Task<string> Status(UpdateRoleStatusInput input)
        {
            await _roleDomainService.UpdateStatus(input);
            if (input.Status == Common.Status.Valid) {
                return "启用角色成功";
            }
            return "禁用角色成功";
        }

        public async Task<string> Update(UpdateRoleInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _roleDomainService.Update(input);
            return "更新角色信息成功";
        }
    }
}
