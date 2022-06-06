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
    public class RoleService : IRoleService
    {
        private IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public bool Add(Role role, out string message)
        {
            var checkData = _roleRepository.AsQueryable().FirstOrDefault(x => x.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower());
            if (checkData != null)
            {
                if (checkData.Recstatus == ActionConst.Authorize)
                {
                    message = MessageConst.IsExist;
                }
                else if (checkData.Recstatus == ActionConst.Cancel)
                {
                    message = MessageConst.IsExistCancel;
                }
                else
                {
                    message = MessageConst.IsExistUnknown;
                }
                return false;
            }
            else
            {
                role.CreatedDate = DateTime.Now;
                role.Recstatus = ActionConst.Authorize;
                _roleRepository.Add(role);
                message = MessageConst.Insert;
                return true;
            }
        }

        public List<Role> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<Role>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _roleRepository.AsQueryable()
                        .OrderByDescending(o => o.RoleId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _roleRepository.AsQueryable().Count();
            }
            else
            {
                data = _roleRepository.AsQueryable().Where(x => x.RoleName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))
                        .OrderByDescending(o => o.RoleId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _roleRepository.AsQueryable().Where(x => x.RoleName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            return data;
        }

        public bool Update(Role role, out string message)
        {
            var updateRole = _roleRepository.AsQueryable().FirstOrDefault(x => x.RoleId == role.RoleId);
            if (updateRole != null)
            {
                if (updateRole.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower())
                {
                    updateRole.Purpose = role.Purpose;
                    updateRole.UpdatedBy = role.UpdatedBy;
                    updateRole.UpdatedDate = DateTime.Now;
                    _roleRepository.Update(updateRole);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    var alreadyExist = _roleRepository.AsQueryable().FirstOrDefault(x => x.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower());
                    if (alreadyExist != null)
                    {
                        if (alreadyExist.Recstatus == ActionConst.Authorize)
                        {
                            message = MessageConst.IsExist;
                        }
                        else if (alreadyExist.Recstatus == ActionConst.Cancel)
                        {
                            message = MessageConst.IsExistCancel;
                        }
                        else
                        {
                            message = MessageConst.IsExistUnknown;
                        }
                        return false;
                    }
                    else
                    {
                        updateRole.RoleName = role.RoleName;
                        updateRole.Purpose = role.Purpose;
                        updateRole.UpdatedBy = role.UpdatedBy;
                        updateRole.UpdatedDate = DateTime.Now;
                        _roleRepository.Update(updateRole);
                        message = MessageConst.Update;
                        return true;
                    }
                }
            }
            else
            {
                message = MessageConst.NotFound;
                return false;
            }
        }

        public Role GetById(long roleId)
        {
            return _roleRepository.AsQueryable().FirstOrDefault(x => x.RoleId == roleId);
        }

        public bool Delete(Role role)
        {
            var updateRole = _roleRepository.AsQueryable().FirstOrDefault(x => x.RoleId == role.RoleId);
            if (updateRole != null)
            {
                updateRole.Recstatus = ActionConst.Cancel;
                updateRole.UpdatedBy = role.UpdatedBy;
                updateRole.UpdatedDate = DateTime.Now;
                _roleRepository.Update(updateRole);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(Role role)
        {
            var updateRole = _roleRepository.AsQueryable().FirstOrDefault(x => x.RoleId == role.RoleId);
            if (updateRole != null)
            {
                updateRole.Recstatus = ActionConst.Authorize;
                updateRole.UpdatedBy = role.UpdatedBy;
                updateRole.UpdatedDate = DateTime.Now;
                _roleRepository.Update(updateRole);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Role> GetAllForDropdown()
        {
            var data = new List<Role>();
            data = _roleRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).Select(x => new Role()
                    {
                        RoleId = x.RoleId,
                        RoleName = x.RoleName
                    })
                    .OrderBy(o => o.RoleName).ToList();
            return data;
        }
    }
}
