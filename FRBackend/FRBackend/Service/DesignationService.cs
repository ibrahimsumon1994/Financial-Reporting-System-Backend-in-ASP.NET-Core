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
    public class DesignationService : IDesignationService
    {
        private IDesignationRepository _designationRepository;
        public DesignationService(IDesignationRepository designationRepository)
        {
            _designationRepository = designationRepository;
        }

        public bool Add(Designation designation, out string message)
        {
            var checkData = _designationRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == designation.NameEnglish.Trim().ToLower());
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
                designation.CreatedDate = DateTime.Now;
                designation.Recstatus = ActionConst.Authorize;
                _designationRepository.Add(designation);
                message = MessageConst.Insert;
                return true;
            }
        }

        public List<Designation> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<Designation>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _designationRepository.AsQueryable().Include(x => x.Department)
                        .OrderByDescending(o => o.DesignationId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _designationRepository.AsQueryable().Count();
            }
            else
            {
                data = _designationRepository.AsQueryable().Include(x => x.Department).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Department.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))
                        .OrderByDescending(o => o.DesignationId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _designationRepository.AsQueryable().Include(x => x.Department).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Department.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            return data;
        }

        public bool Update(Designation designation, out string message)
        {
            var updateDesignation = _designationRepository.AsQueryable().FirstOrDefault(x => x.DesignationId == designation.DesignationId);
            if (updateDesignation != null)
            {
                if (updateDesignation.NameEnglish.Trim().ToLower() == designation.NameEnglish.Trim().ToLower())
                {
                    updateDesignation.NameBangla = designation.NameBangla;
                    updateDesignation.DepartmentId = designation.DepartmentId;
                    updateDesignation.UpdatedBy = designation.UpdatedBy;
                    updateDesignation.UpdatedDate = DateTime.Now;
                    _designationRepository.Update(updateDesignation);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    var alreadyExist = _designationRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == designation.NameEnglish.Trim().ToLower());
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
                        updateDesignation.NameEnglish = designation.NameEnglish;
                        updateDesignation.NameBangla = designation.NameBangla;
                        updateDesignation.DepartmentId = designation.DepartmentId;
                        updateDesignation.UpdatedBy = designation.UpdatedBy;
                        updateDesignation.UpdatedDate = DateTime.Now;
                        _designationRepository.Update(updateDesignation);
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

        public Designation GetById(long designationId)
        {
            return _designationRepository.AsQueryable().FirstOrDefault(x => x.DesignationId == designationId);
        }

        public bool Delete(Designation designation)
        {
            var updateDesignation = _designationRepository.AsQueryable().FirstOrDefault(x => x.DesignationId == designation.DesignationId);
            if (updateDesignation != null)
            {
                updateDesignation.Recstatus = ActionConst.Cancel;
                updateDesignation.UpdatedBy = designation.UpdatedBy;
                updateDesignation.UpdatedDate = DateTime.Now;
                _designationRepository.Update(updateDesignation);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(Designation designation)
        {
            var updateDesignation = _designationRepository.AsQueryable().FirstOrDefault(x => x.DesignationId == designation.DesignationId);
            if (updateDesignation != null)
            {
                updateDesignation.Recstatus = ActionConst.Authorize;
                updateDesignation.UpdatedBy = designation.UpdatedBy;
                updateDesignation.UpdatedDate = DateTime.Now;
                _designationRepository.Update(updateDesignation);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Designation> GetAllForDropdown()
        {
            var data = new List<Designation>();
            data = _designationRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize)
                        .OrderBy(o => o.NameEnglish).ToList();
            return data;
        }
    }
}
