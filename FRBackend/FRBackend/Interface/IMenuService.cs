using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IMenuService
    {
        bool Add(Menu menu);
        List<MenuModel> GetAll(PagingParam param, out int total);
        bool Update(Menu menu, out string message);
        Menu GetById(long menuId);
        bool Delete(Menu menu);
        bool Restore(Menu menu);
        List<MenuPermission> MenuListPermission(string userId);
        List<Menu> GetParentMenuForDropdown();
        List<Menu> GetNonMatchMenuByRoleDropdown(long roleId);
        List<Menu> GetNonMatchMenuByRoleWithSelectedMenuDropdown(long roleId, long menuId);
        List<MenuProcessing> GetMenuList(string userId);
    }
}
