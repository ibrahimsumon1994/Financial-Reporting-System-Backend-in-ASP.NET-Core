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
    public class UserWiseUnitPermissionService : IUserWiseUnitPermissionService
    {
        private IUserWiseUnitPermissionRepository _userWiseUnitPermissionRepository;
        public UserWiseUnitPermissionService(IUserWiseUnitPermissionRepository userWiseUnitPermissionRepository)
        {
            _userWiseUnitPermissionRepository = userWiseUnitPermissionRepository;
        }

        public bool Add(UserWiseUnitPermission userWiseUnitPermission)
        {
            var checkData = _userWiseUnitPermissionRepository.AsQueryable().FirstOrDefault(x => x.UnitId == userWiseUnitPermission.UnitId && x.UserId == userWiseUnitPermission.UserId && x.Recstatus == ActionConst.Authorize);
            if (checkData != null)
            {
                return false;
            }
            else
            {
                userWiseUnitPermission.CreatedDate = DateTime.Now;
                userWiseUnitPermission.Recstatus = ActionConst.Authorize;
                _userWiseUnitPermissionRepository.Add(userWiseUnitPermission);
                return true;
            }
        }

        public List<UserWiseUnitPermission> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<UserWiseUnitPermission>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _userWiseUnitPermissionRepository.AsQueryable().Include(x => x.Unit).Include(x => x.User).Where(x => x.Recstatus == ActionConst.Authorize)
                        .OrderByDescending(o => o.UnitPermissionId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _userWiseUnitPermissionRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).Count();
            }
            else
            {
                data = _userWiseUnitPermissionRepository.AsQueryable().Include(x => x.Unit).Include(x => x.User).Where(x => x.Recstatus == ActionConst.Authorize && (x.Unit.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.User.UserId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())))
                        .OrderByDescending(o => o.UnitPermissionId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _userWiseUnitPermissionRepository.AsQueryable().Include(x => x.Unit).Include(x => x.User).Where(x => x.Recstatus == ActionConst.Authorize && (x.Unit.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.User.UserId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))).Count();
            }
            return data;
        }

        public bool Update(UserWiseUnitPermission userWiseUnitPermission, out string message)
        {
            var alreadyExist = _userWiseUnitPermissionRepository.AsQueryable().FirstOrDefault(x => x.UnitId == userWiseUnitPermission.UnitId && x.UserId == userWiseUnitPermission.UserId && x.Recstatus == ActionConst.Authorize);
            if (alreadyExist == null)
            {
                var updateUserWiseUnitPermission = _userWiseUnitPermissionRepository.AsQueryable().FirstOrDefault(x => x.UnitPermissionId == userWiseUnitPermission.UnitPermissionId);
                if (updateUserWiseUnitPermission != null)
                {
                    updateUserWiseUnitPermission.UnitId = userWiseUnitPermission.UnitId;
                    updateUserWiseUnitPermission.UserId = userWiseUnitPermission.UserId;
                    updateUserWiseUnitPermission.UpdatedBy = userWiseUnitPermission.UpdatedBy;
                    updateUserWiseUnitPermission.UpdatedDate = DateTime.Now;
                    _userWiseUnitPermissionRepository.Update(updateUserWiseUnitPermission);
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

        public UserWiseUnitPermission GetById(long unitPermissionId)
        {
            return _userWiseUnitPermissionRepository.AsQueryable().FirstOrDefault(x => x.UnitPermissionId == unitPermissionId);
        }

        public bool Delete(UserWiseUnitPermission userWiseUnitPermission)
        {
            var updateUserWiseUnitPermission = _userWiseUnitPermissionRepository.AsQueryable().FirstOrDefault(x => x.UnitPermissionId == userWiseUnitPermission.UnitPermissionId);
            if (updateUserWiseUnitPermission != null)
            {
                updateUserWiseUnitPermission.Recstatus = ActionConst.Cancel;
                updateUserWiseUnitPermission.UpdatedBy = userWiseUnitPermission.UpdatedBy;
                updateUserWiseUnitPermission.UpdatedDate = DateTime.Now;
                _userWiseUnitPermissionRepository.Update(updateUserWiseUnitPermission);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<UserWiseUnitPermission> GetUserWiseUnitForDropdown(string userId)
        {
            var data = new List<UserWiseUnitPermission>();
            data = _userWiseUnitPermissionRepository.AsQueryable().Include(x => x.Unit).Where(x => x.UserId == Convert.ToInt32(userId) && x.Recstatus == ActionConst.Authorize)
                        .OrderBy(o => o.Unit.NameEnglish).ToList();
            return data;
        }
    }
}
