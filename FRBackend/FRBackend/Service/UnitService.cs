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
    public class UnitService : IUnitService
    {
        private IUnitRepository _unitRepository;
        public UnitService(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public bool Add(Unit unit, out string message)
        {
            var checkData = _unitRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == unit.NameEnglish.Trim().ToLower() && x.GroupId == unit.GroupId);
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
                unit.CreatedDate = DateTime.Now;
                unit.Recstatus = ActionConst.Authorize;
                _unitRepository.Add(unit);
                message = MessageConst.Insert;
                return true;
            }
        }

        public List<Unit> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<Unit>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _unitRepository.AsQueryable().Include(x => x.Group)
                        .OrderByDescending(o => o.UnitId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _unitRepository.AsQueryable().Count();
            }
            else
            {
                data = _unitRepository.AsQueryable().Include(x => x.Group).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Group.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))
                        .OrderByDescending(o => o.UnitId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _unitRepository.AsQueryable().Include(x => x.Group).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.Group.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            return data;
        }

        public bool Update(Unit unit, out string message)
        {
            var updateUnit = _unitRepository.AsQueryable().FirstOrDefault(x => x.UnitId == unit.UnitId);
            if (updateUnit != null)
            {
                if (updateUnit.NameEnglish.Trim().ToLower() == unit.NameEnglish.Trim().ToLower() && updateUnit.GroupId == unit.GroupId)
                {
                    updateUnit.NameBangla = unit.NameBangla;
                    updateUnit.UpdatedBy = unit.UpdatedBy;
                    updateUnit.UpdatedDate = DateTime.Now;
                    _unitRepository.Update(updateUnit);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    var alreadyExist = _unitRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == unit.NameEnglish.Trim().ToLower() && x.GroupId == unit.GroupId);
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
                        updateUnit.NameEnglish = unit.NameEnglish;
                        updateUnit.NameBangla = unit.NameBangla;
                        updateUnit.GroupId = unit.GroupId;
                        updateUnit.UpdatedBy = unit.UpdatedBy;
                        updateUnit.UpdatedDate = DateTime.Now;
                        _unitRepository.Update(updateUnit);
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

        public Unit GetById(long unitId)
        {
            return _unitRepository.AsQueryable().FirstOrDefault(x => x.UnitId == unitId);
        }

        public bool Delete(Unit unit)
        {
            var updateUnit = _unitRepository.AsQueryable().FirstOrDefault(x => x.UnitId == unit.UnitId);
            if (updateUnit != null)
            {
                updateUnit.Recstatus = ActionConst.Cancel;
                updateUnit.UpdatedBy = unit.UpdatedBy;
                updateUnit.UpdatedDate = DateTime.Now;
                _unitRepository.Update(updateUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(Unit unit)
        {
            var updateUnit = _unitRepository.AsQueryable().FirstOrDefault(x => x.UnitId == unit.UnitId);
            if (updateUnit != null)
            {
                updateUnit.Recstatus = ActionConst.Authorize;
                updateUnit.UpdatedBy = unit.UpdatedBy;
                updateUnit.UpdatedDate = DateTime.Now;
                _unitRepository.Update(updateUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Unit> GetAllForDropdown()
        {
            var data = new List<Unit>();
            data = _unitRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize)
                        .OrderBy(o => o.NameEnglish).ToList();
            return data;
        }

        public List<Unit> GetByGroupForDropdown(long groupId)
        {
            var data = new List<Unit>();
            data = _unitRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize && x.GroupId == groupId)
                        .OrderBy(o => o.NameEnglish).ToList();
            return data;
        }
    }
}
