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
    public class RoleWiseMenuAssignService : IRoleWiseMenuAssignService
    {
        private IRoleWiseMenuAssignRepository _roleWiseMenuAssignRepository;
        public RoleWiseMenuAssignService(IRoleWiseMenuAssignRepository roleWiseMenuAssignRepository)
        {
            _roleWiseMenuAssignRepository = roleWiseMenuAssignRepository;
        }

        public bool Add(RoleWiseMenuAssign roleWiseMenuAssign)
        {
            var checkData = _roleWiseMenuAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleId == roleWiseMenuAssign.RoleId && x.MenuId == roleWiseMenuAssign.MenuId && x.Recstatus == ActionConst.Authorize);
            if (checkData != null)
            {
                return false;
            }
            else
            {
                roleWiseMenuAssign.CreatedDate = DateTime.Now;
                roleWiseMenuAssign.Recstatus = ActionConst.Authorize;
                _roleWiseMenuAssignRepository.Add(roleWiseMenuAssign);
                return true;
            }
        }

        public bool AddList(List<RoleWiseMenuAssign> roleWiseMenuAssignList, string loginUser, out string message)
        {
            using (var transaction = _roleWiseMenuAssignRepository.BeginTransaction())
            {
                try
                {
                    foreach (var roleWiseMenuAssign in roleWiseMenuAssignList)
                    {
                        roleWiseMenuAssign.Recstatus = ActionConst.Authorize;
                        roleWiseMenuAssign.CreatedBy = Convert.ToInt32(loginUser);
                        roleWiseMenuAssign.CreatedDate = DateTime.Now;
                        _roleWiseMenuAssignRepository.Add(roleWiseMenuAssign);
                    }
                    message = MessageConst.Insert;
                    _roleWiseMenuAssignRepository.Commit(transaction);
                    return true;
                }
                catch (Exception ex)
                {
                    _roleWiseMenuAssignRepository.Rollback(transaction);
                    if (ex.InnerException != null)
                    {
                        message = ex.InnerException.Message;
                    }
                    else
                    {
                        message = ex.Message;
                    }
                    return false;
                }
            }
        }

        public List<RoleWiseMenuAssign> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<RoleWiseMenuAssign>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _roleWiseMenuAssignRepository.AsQueryable().Include(x => x.Role).Include(x => x.Menu).Where(x => x.Recstatus == ActionConst.Authorize)
                        .OrderByDescending(o => o.RoleWiseMenuAssignId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _roleWiseMenuAssignRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).Count();
            }
            else
            {
                data = _roleWiseMenuAssignRepository.AsQueryable().Include(x => x.Role).Include(x => x.Menu).Where(x => x.Recstatus == ActionConst.Authorize && (x.Role.RoleName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Menu.MenuName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())))
                        .OrderByDescending(o => o).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _roleWiseMenuAssignRepository.AsQueryable().Include(x => x.Role).Include(x => x.Menu).Where(x => x.Recstatus == ActionConst.Authorize && (x.Role.RoleName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Menu.MenuName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))).Count();
            }
            return data;
        }

        public bool Update(RoleWiseMenuAssign roleWiseMenuAssign, out string message)
        {
            var updateRoleWiseMenuAssign = _roleWiseMenuAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleWiseMenuAssignId == roleWiseMenuAssign.RoleWiseMenuAssignId);
            if (updateRoleWiseMenuAssign != null)
            {
                updateRoleWiseMenuAssign.RoleId = roleWiseMenuAssign.RoleId;
                updateRoleWiseMenuAssign.MenuId = roleWiseMenuAssign.MenuId;
                updateRoleWiseMenuAssign.CreateYn = roleWiseMenuAssign.CreateYn;
                updateRoleWiseMenuAssign.EditYn = roleWiseMenuAssign.EditYn;
                updateRoleWiseMenuAssign.ViewDetailYn = roleWiseMenuAssign.ViewDetailYn;
                updateRoleWiseMenuAssign.DeleteYn = roleWiseMenuAssign.DeleteYn;
                updateRoleWiseMenuAssign.AuthYn = roleWiseMenuAssign.AuthYn;
                updateRoleWiseMenuAssign.UpdatedBy = roleWiseMenuAssign.UpdatedBy;
                updateRoleWiseMenuAssign.UpdatedDate = DateTime.Now;
                _roleWiseMenuAssignRepository.Update(updateRoleWiseMenuAssign);
                message = MessageConst.Update;
                return true;
            }
            else
            {
                message = MessageConst.NotFound;
                return false;
            }
        }

        public RoleWiseMenuAssign GetById(long roleWiseMenuAssignId)
        {
            return _roleWiseMenuAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleWiseMenuAssignId == roleWiseMenuAssignId);
        }

        public bool Delete(RoleWiseMenuAssign roleWiseMenuAssign)
        {
            var updateRoleWiseMenuAssign = _roleWiseMenuAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleWiseMenuAssignId == roleWiseMenuAssign.RoleWiseMenuAssignId);
            if (updateRoleWiseMenuAssign != null)
            {
                updateRoleWiseMenuAssign.Recstatus = ActionConst.Cancel;
                updateRoleWiseMenuAssign.UpdatedBy = roleWiseMenuAssign.UpdatedBy;
                updateRoleWiseMenuAssign.UpdatedDate = DateTime.Now;
                _roleWiseMenuAssignRepository.Update(updateRoleWiseMenuAssign);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
