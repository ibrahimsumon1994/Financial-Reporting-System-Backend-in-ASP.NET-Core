using Common.Helpers;
using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Helpers;
using FRBackend.Interface;
using FRBackend.Model;
using FRBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Service
{
    public class MenuService : IMenuService
    {
        private IMenuRepository _menuRepository;
        private IMenuPermissionRepository _menuPermissionRepository;
        private IRoleWiseMenuAssignRepository _roleWiseMenuAssignRepository;
        public MenuService(IMenuRepository menuRepository, IMenuPermissionRepository menuPermissionRepository, IRoleWiseMenuAssignRepository roleWiseMenuAssignRepository)
        {
            _menuRepository = menuRepository;
            _menuPermissionRepository = menuPermissionRepository;
            _roleWiseMenuAssignRepository = roleWiseMenuAssignRepository;
        }

        public bool Add(Menu menu)
        {
            menu.CreatedDate = DateTime.Now;
            menu.Recstatus = ActionConst.Authorize;
            _menuRepository.Add(menu);
            return true;
        }

        public List<MenuModel> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<MenuModel>();
            var menus = new List<Menu>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                menus = _menuRepository.AsQueryable().Include(x => x.MenuType)
                            .OrderByDescending(o => o.MenuId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                
                total = _menuRepository.AsQueryable().Count();
            }
            else
            {
                menus = _menuRepository.AsQueryable().Include(x => x.MenuType).Where(x => x.MenuName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.MenuLocation.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.ReportPath.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.MenuType.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))
                        .OrderByDescending(o => o.MenuId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _menuRepository.AsQueryable().Include(x => x.MenuType).Where(x => x.MenuName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.MenuLocation.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.ReportPath.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.MenuType.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            foreach (var menu in menus)
            {
                var obj = new MenuModel();
                obj.MenuId = menu.MenuId;
                obj.MenuName = menu.MenuName;
                obj.MenuLocation = menu.MenuLocation;
                obj.ReportPath = menu.ReportPath;
                obj.ParentId = menu.ParentId;
                var parentData = _menuRepository.AsQueryable().FirstOrDefault(x => x.MenuId == menu.ParentId);
                if (parentData != null)
                {
                    obj.ParentName = parentData.MenuName;
                }
                obj.MenuIcon = menu.MenuIcon;
                obj.MenuSequence = menu.MenuSequence;
                obj.MenuTypeId = menu.MenuTypeId;
                if (menu.MenuType != null)
                {
                    obj.MenuType = menu.MenuType.NameEnglish;
                }
                obj.Recstatus = menu.Recstatus;
                data.Add(obj);
            }
            return data;
        }

        public bool Update(Menu menu, out string message)
        {
            var updateMenu = _menuRepository.AsQueryable().FirstOrDefault(x => x.MenuId == menu.MenuId);
            if (updateMenu != null)
            {
                updateMenu.MenuName = menu.MenuName;
                updateMenu.MenuLocation = menu.MenuLocation;
                updateMenu.MenuTypeId = menu.MenuTypeId;
                updateMenu.ReportPath = menu.ReportPath;
                updateMenu.ParentId = menu.ParentId;
                updateMenu.MenuSequence = menu.MenuSequence;
                updateMenu.MenuIcon = menu.MenuIcon;
                updateMenu.UpdatedBy = menu.UpdatedBy;
                updateMenu.UpdatedDate = DateTime.Now;
                _menuRepository.Update(updateMenu);
                message = MessageConst.Update;
                return true;
            }
            else
            {
                message = MessageConst.NotFound;
                return false;
            }
        }

        public Menu GetById(long menuId)
        {
            return _menuRepository.AsQueryable().FirstOrDefault(x => x.MenuId == menuId);
        }

        public bool Delete(Menu menu)
        {
            var updateMenu = _menuRepository.AsQueryable().FirstOrDefault(x => x.MenuId == menu.MenuId);
            if (updateMenu != null)
            {
                updateMenu.Recstatus = ActionConst.Cancel;
                updateMenu.UpdatedBy = menu.UpdatedBy;
                updateMenu.UpdatedDate = DateTime.Now;
                _menuRepository.Update(updateMenu);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(Menu menu)
        {
            var updateMenu = _menuRepository.AsQueryable().FirstOrDefault(x => x.MenuId == menu.MenuId);
            if (updateMenu != null)
            {
                updateMenu.Recstatus = ActionConst.Authorize;
                updateMenu.UpdatedBy = menu.UpdatedBy;
                updateMenu.UpdatedDate = DateTime.Now;
                _menuRepository.Update(updateMenu);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<MenuPermission> MenuListPermission(string userId)
        {
            return _menuPermissionRepository.AsQueryable().Where(x => x.UserId == userId).ToList();
        }

        public List<Menu> GetParentMenuForDropdown()
        {
            var data = new List<Menu>();
            data = _menuRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).Select(x => new Menu() 
                    {
                        MenuId = x.MenuId,
                        MenuName = x.MenuName
                    })
                   .OrderBy(o => o.MenuName).ToList();
            return data;
        }

        public List<Menu> GetNonMatchMenuByRoleDropdown(long roleId)
        {
            var menuData = _menuRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).ToList();
            var roleWiseMenuAssignData = _roleWiseMenuAssignRepository.AsQueryable().Where(x => x.RoleId == roleId && x.Recstatus == ActionConst.Authorize).ToList();
            var nonMatchMenuByRoleDropdown = menuData.Where(a => !roleWiseMenuAssignData.Select(b => b.MenuId).Contains(a.MenuId)).ToList();
            return nonMatchMenuByRoleDropdown;
        }

        public List<Menu> GetNonMatchMenuByRoleWithSelectedMenuDropdown(long roleId, long menuId)
        {
            var menuData = _menuRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).ToList();
            var roleWiseMenuAssignData = _roleWiseMenuAssignRepository.AsQueryable().Where(x => x.RoleId == roleId && x.Recstatus == ActionConst.Authorize).ToList();
            var nonMatchMenuByRoleDropdown = menuData.Where(a => !roleWiseMenuAssignData.Select(b => b.MenuId).Contains(a.MenuId)).ToList();
            var editedMenu = _menuRepository.AsQueryable().FirstOrDefault(x => x.MenuId == menuId);
            if (editedMenu != null)
            {
                nonMatchMenuByRoleDropdown.Add(editedMenu);
            }
            return nonMatchMenuByRoleDropdown;
        }

        public List<MenuProcessing> GetMenuList(string userId)
        {
            var returnData = new List<MenuProcessing>();
            var menus = _menuPermissionRepository.AsQueryable().Where(x => x.UserId == userId).ToList();
            var munuList = menus.OrderBy(i => i.MenuSequence).ToList();
            var parent = munuList.Where(p => p.ParentId == 0).OrderBy(i => i.MenuSequence).ToList();
            foreach (var item in parent)
            {
                var parentObj = GenerateMenu(item);
                var chield = munuList.Where(p => p.ParentId == item.MenuId).OrderBy(i => i.MenuSequence).ToList();
                foreach (var itemChild in chield)
                {
                    var childObj = GenerateMenu(itemChild);
                    var childChildNode = munuList.Where(w => w.ParentId == itemChild.MenuId).OrderBy(o => o.MenuSequence).ToList();
                    foreach (var lastChild in childChildNode)
                    {
                        var childChildObj = GenerateMenu(lastChild);
                        var childChildChildNode = munuList.Where(w => w.ParentId == lastChild.MenuId).OrderBy(o => o.MenuSequence).ToList();
                        foreach (var lastChildChild in childChildChildNode)
                        {
                            var childChildChildObj = GenerateMenu(lastChildChild);
                            childChildObj.children.Add(childChildChildObj);
                        }
                        childObj.children.Add(childChildObj);
                    }
                    parentObj.children.Add(childObj);
                }
                returnData.Add(parentObj);
            }
            return returnData;
        }

        private MenuProcessing GenerateMenu(MenuPermission item)
        {
            var obj = new MenuProcessing();
            obj.MenuId = item.MenuId;
            obj.ParentId = item.ParentId;
            obj.MenuName = item.MenuName;
            obj.MenuLocation = item.MenuLocation;
            obj.MenuSequence = item.MenuSequence;
            obj.MenuIcon = item.MenuIcon;
            obj.RoleId = item.RoleId;
            obj.UserId = item.UserId;
            obj.CreateYn = item.CreateYn;
            obj.EditYn = item.EditYn;
            obj.DeleteYn = item.DeleteYn;
            obj.ViewDetailYn = item.ViewDetailYn;
            obj.AuthYn = item.AuthYn;
            return obj;
        }
    }
}
