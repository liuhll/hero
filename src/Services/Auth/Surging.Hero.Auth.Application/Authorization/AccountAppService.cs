using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Session;
using Surging.Cloud.Dapper.Repositories;
using Surging.Cloud.Domain.Trees;
using Surging.Cloud.ProxyGenerator;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Authorization;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using Surging.Hero.Common.Runtime.Session;

namespace Surging.Hero.Auth.Application.Authorization
{
    [ModuleName(AuthConstant.V1.AccountMoudleName, Version = AuthConstant.V1.Version)]
    public class AccountAppService : ProxyServiceBase, IAccountAppService
    {
        private readonly ILoginManager _loginManager;
        private readonly ISurgingSession _surgingSession;
        private readonly IUserDomainService _userDomainService;
        private readonly IDapperRepository<Menu, long> _menuRepository;

        public AccountAppService(ILoginManager loginManager,
            IUserDomainService userDomainService,
            IDapperRepository<Menu, long> menuRepository)
        {
            _loginManager = loginManager;
            _userDomainService = userDomainService;
            _menuRepository = menuRepository;
            _surgingSession = NullSurgingSession.Instance;
        }

        public async Task<LoginUserInfo> GetLoginUser()
        {
            if (_surgingSession == null || !_surgingSession.UserId.HasValue) throw new BusinessException("您当前没有登录系统");
            var userInfo = await _userDomainService.GetUserNormInfoById(_surgingSession.UserId.Value);
            return userInfo.MapTo<LoginUserInfo>();
        }

        public async Task<IEnumerable<ITree<GetUserMenuTreeOutput>>> GetUserTreeMenu()
        {
            if (_surgingSession == null || !_surgingSession.UserId.HasValue) throw new BusinessException("您当前没有登录系统");
            var userMenus = await _userDomainService.GetUserMenu(_surgingSession.UserId.Value);
            var treeOutputs = userMenus.MapTo<IEnumerable<GetUserMenuTreeOutput>>();
            return treeOutputs.BuildTree();
        }

        public async Task<IEnumerable<GetUserMenuOutput>> GetUserMenu()
        {
            if (_surgingSession == null || !_surgingSession.UserId.HasValue) throw new BusinessException("您当前没有登录系统");
            var userMenus = await _userDomainService.GetUserMenu(_surgingSession.UserId.Value);
            var outputs = userMenus.MapTo<IEnumerable<GetUserMenuOutput>>();
            return outputs;
        }

        public async Task<IEnumerable<GetUserOperationOutput>> GetUserOperation(long menuId)
        {
            if (_surgingSession == null || !_surgingSession.UserId.HasValue) throw new BusinessException("您当前没有登录系统");
            var userOperations = await _userDomainService.GetUserOperation(_surgingSession.UserId.Value, menuId);
            return userOperations.MapTo<IEnumerable<GetUserOperationOutput>>();
        }

        public async Task<IEnumerable<GetUserOperationOutput>> GetUserOperationByMenuName(string menuName)
        {
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.Name == menuName);
            if (menu == null)
            {
                throw new BusinessException($"系统中不存在{menuName}的菜单");
            }
            return await GetUserOperation(menu.Id);
        }

        public async Task<IDictionary<string, object>> Login(LoginInput input)
        {
            return await _loginManager.Login(input);
        }
    }
}