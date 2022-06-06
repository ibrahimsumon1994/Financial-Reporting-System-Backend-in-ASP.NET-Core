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
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public bool Add(Department department, out string message)
        {
            var checkData = _departmentRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == department.NameEnglish.Trim().ToLower() && x.GroupId == department.GroupId && x.UnitId == department.UnitId);
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
                department.CreatedDate = DateTime.Now;
                department.Recstatus = ActionConst.Authorize;
                _departmentRepository.Add(department);
                message = MessageConst.Insert;
                return true;
            }
        }

        public List<Department> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<Department>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _departmentRepository.AsQueryable().Include(x => x.Unit).Include(x => x.Group)
                        .OrderByDescending(o => o.DepartmentId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _departmentRepository.AsQueryable().Count();
            }
            else
            {
                data = _departmentRepository.AsQueryable().Include(x => x.Unit).Include(x => x.Group).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Unit.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Group.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))
                        .OrderByDescending(o => o.DepartmentId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _departmentRepository.AsQueryable().Include(x => x.Unit).Include(x => x.Group).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Unit.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Group.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            return data;
        }

        public bool Update(Department department, out string message)
        {
            var updateDepartment = _departmentRepository.AsQueryable().FirstOrDefault(x => x.DepartmentId == department.DepartmentId);
            if (updateDepartment != null)
            {
                if (updateDepartment.NameEnglish.Trim().ToLower() == department.NameEnglish.Trim().ToLower() && updateDepartment.UnitId == department.UnitId && updateDepartment.GroupId == department.GroupId)
                {
                    updateDepartment.NameBangla = department.NameBangla;
                    updateDepartment.UpdatedBy = department.UpdatedBy;
                    updateDepartment.UpdatedDate = DateTime.Now;
                    _departmentRepository.Update(updateDepartment);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    var alreadyExist = _departmentRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == department.NameEnglish.Trim().ToLower() && x.GroupId == department.GroupId && x.UnitId == department.UnitId);
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
                        updateDepartment.NameEnglish = department.NameEnglish;
                        updateDepartment.NameBangla = department.NameBangla;
                        updateDepartment.GroupId = department.GroupId;
                        updateDepartment.UnitId = department.UnitId;
                        updateDepartment.UpdatedBy = department.UpdatedBy;
                        updateDepartment.UpdatedDate = DateTime.Now;
                        _departmentRepository.Update(updateDepartment);
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

        public Department GetById(long departmentId)
        {
            return _departmentRepository.AsQueryable().FirstOrDefault(x => x.DepartmentId == departmentId);
        }

        public bool Delete(Department department)
        {
            var updateDepartment = _departmentRepository.AsQueryable().FirstOrDefault(x => x.DepartmentId == department.DepartmentId);
            if (updateDepartment != null)
            {
                updateDepartment.Recstatus = ActionConst.Cancel;
                updateDepartment.UpdatedBy = department.UpdatedBy;
                updateDepartment.UpdatedDate = DateTime.Now;
                _departmentRepository.Update(updateDepartment);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(Department department)
        {
            var updateDepartment = _departmentRepository.AsQueryable().FirstOrDefault(x => x.DepartmentId == department.DepartmentId);
            if (updateDepartment != null)
            {
                updateDepartment.Recstatus = ActionConst.Authorize;
                updateDepartment.UpdatedBy = department.UpdatedBy;
                updateDepartment.UpdatedDate = DateTime.Now;
                _departmentRepository.Update(updateDepartment);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Department> GetAllForDropdown()
        {
            var data = new List<Department>();
            data = _departmentRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize)
                        .OrderBy(o => o.NameEnglish).ToList();
            return data;
        }

        public List<Department> GetByUnitForDropdown(long unitId)
        {
            var data = new List<Department>();
            data = _departmentRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize && x.UnitId == unitId)
                        .OrderBy(o => o.NameEnglish).ToList();
            return data;
        }
    }
}
