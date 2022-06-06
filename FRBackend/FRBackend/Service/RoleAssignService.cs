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
    public class RoleAssignService : IRoleAssignService
    {
        private IRoleAssignRepository _roleAssignRepository;
        public RoleAssignService(IRoleAssignRepository roleAssignRepository)
        {
            _roleAssignRepository = roleAssignRepository;
        }

        public bool Add(RoleAssign roleAssign)
        {
            var checkData = _roleAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleId == roleAssign.RoleId && x.UserId == roleAssign.UserId && x.Recstatus == ActionConst.Authorize);
            if (checkData != null)
            {
                return false;
            }
            else
            {
                roleAssign.CreatedDate = DateTime.Now;
                roleAssign.Recstatus = ActionConst.Authorize;
                _roleAssignRepository.Add(roleAssign);
                return true;
            }
        }

        public List<RoleAssign> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<RoleAssign>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _roleAssignRepository.AsQueryable().Include(x => x.Role).Include(x => x.User).Where(x => x.Recstatus == ActionConst.Authorize)
                        .OrderByDescending(o => o.RoleAssignId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _roleAssignRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).Count();
            }
            else
            {
                data = _roleAssignRepository.AsQueryable().Include(x => x.Role).Include(x => x.User).Where(x => x.Recstatus == ActionConst.Authorize && (x.Role.RoleName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.User.UserId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())))
                        .OrderByDescending(o => o.RoleAssignId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _roleAssignRepository.AsQueryable().Include(x => x.Role).Include(x => x.User).Where(x => x.Recstatus == ActionConst.Authorize && (x.Role.RoleName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.User.UserId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))).Count();
            }
            return data;
        }

        public bool Update(RoleAssign roleAssign, out string message)
        {
            var alreadyExist = _roleAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleId == roleAssign.RoleId && x.UserId == roleAssign.UserId && x.Recstatus == ActionConst.Authorize);
            if (alreadyExist == null)
            {
                var updateRoleAssign = _roleAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleAssignId == roleAssign.RoleAssignId);
                if (updateRoleAssign != null)
                {
                    updateRoleAssign.RoleId = roleAssign.RoleId;
                    updateRoleAssign.UserId = roleAssign.UserId;
                    updateRoleAssign.UpdatedBy = roleAssign.UpdatedBy;
                    updateRoleAssign.UpdatedDate = DateTime.Now;
                    _roleAssignRepository.Update(updateRoleAssign);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    message = MessageConst.NotFound;
                    return false;
                }
            }
            else 
            {
                message = MessageConst.IsExist;
                return false;
            }
        }

        public RoleAssign GetById(long roleAssignId)
        {
            return _roleAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleAssignId == roleAssignId);
        }

        public bool Delete(RoleAssign roleAssign)
        {
            var updateRoleAssign = _roleAssignRepository.AsQueryable().FirstOrDefault(x => x.RoleAssignId == roleAssign.RoleAssignId);
            if (updateRoleAssign != null)
            {
                updateRoleAssign.Recstatus = ActionConst.Cancel;
                updateRoleAssign.UpdatedBy = roleAssign.UpdatedBy;
                updateRoleAssign.UpdatedDate = DateTime.Now;
                _roleAssignRepository.Update(updateRoleAssign);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
