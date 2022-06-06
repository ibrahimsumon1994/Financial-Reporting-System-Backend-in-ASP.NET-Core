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
    public class CommonCodeService : ICommonCodeService
    {
        private ICommonCodeRepository _commonCodeRepository;
        public CommonCodeService(ICommonCodeRepository commonCodeRepository)
        {
            _commonCodeRepository = commonCodeRepository;
        }

        public bool Add(CommonCode commonCode, out string message)
        {
            var checkData = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == commonCode.NameEnglish.Trim().ToLower() && x.Type.Trim().ToLower() == commonCode.Type.Trim().ToLower());
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
                commonCode.CreatedDate = DateTime.Now;
                commonCode.Recstatus = ActionConst.Authorize;
                _commonCodeRepository.Add(commonCode);
                message = MessageConst.Insert;
                return true;
            }
        }

        public List<CommonCode> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<CommonCode>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _commonCodeRepository.AsQueryable().Include(x => x.Department)
                            .OrderByDescending(o => o.CommonCodeId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                
                total = _commonCodeRepository.AsQueryable().Count();
            }
            else
            {
                data = _commonCodeRepository.AsQueryable().Include(x => x.Department).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Code.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.Type.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))
                        .OrderByDescending(o => o.CommonCodeId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _commonCodeRepository.AsQueryable().Include(x => x.Department).Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Code.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.Type.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            return data;
        }

        public bool Update(CommonCode commonCode, out string message)
        {
            var updateCommonCode = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.CommonCodeId == commonCode.CommonCodeId);
            if (updateCommonCode != null)
            {
                if (updateCommonCode.NameEnglish.Trim().ToLower() == commonCode.NameEnglish.Trim().ToLower() && updateCommonCode.Type == commonCode.Type)
                {
                    updateCommonCode.NameBangla = commonCode.NameBangla;
                    updateCommonCode.Code = commonCode.Code;
                    updateCommonCode.DepartmentId = commonCode.DepartmentId;
                    updateCommonCode.UpdatedBy = commonCode.UpdatedBy;
                    updateCommonCode.UpdatedDate = DateTime.Now;
                    _commonCodeRepository.Update(updateCommonCode);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    var alreadyExist = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == commonCode.NameEnglish.Trim().ToLower() && x.Type == commonCode.Type);
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
                        updateCommonCode.NameEnglish = commonCode.NameEnglish;
                        updateCommonCode.NameBangla = commonCode.NameBangla;
                        updateCommonCode.Type = commonCode.Type;
                        updateCommonCode.Code = commonCode.Code;
                        updateCommonCode.DepartmentId = commonCode.DepartmentId;
                        updateCommonCode.UpdatedBy = commonCode.UpdatedBy;
                        updateCommonCode.UpdatedDate = DateTime.Now;
                        _commonCodeRepository.Update(updateCommonCode);
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

        public CommonCode GetById(long commonCodeId)
        {
            return _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.CommonCodeId == commonCodeId);
        }

        public bool Delete(CommonCode commonCode)
        {
            var updateCommonCode = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.CommonCodeId == commonCode.CommonCodeId);
            if (updateCommonCode != null)
            {
                updateCommonCode.Recstatus = ActionConst.Cancel;
                updateCommonCode.UpdatedBy = commonCode.UpdatedBy;
                updateCommonCode.UpdatedDate = DateTime.Now;
                _commonCodeRepository.Update(updateCommonCode);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(CommonCode commonCode)
        {
            var updateCommonCode = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.CommonCodeId == commonCode.CommonCodeId);
            if (updateCommonCode != null)
            {
                updateCommonCode.Recstatus = ActionConst.Authorize;
                updateCommonCode.UpdatedBy = commonCode.UpdatedBy;
                updateCommonCode.UpdatedDate = DateTime.Now;
                _commonCodeRepository.Update(updateCommonCode);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<CommonCode> GetByType(string type)
        {
            var data = new List<CommonCode>();
            data = _commonCodeRepository.AsQueryable().Where(x => x.Type == type && x.Recstatus == ActionConst.Authorize).Select(x => new CommonCode() 
                    {
                        CommonCodeId = x.CommonCodeId,
                        NameEnglish = x.NameEnglish
                    })
                    .OrderBy(o => o.NameEnglish).ToList();
            return data;
        }
    }
}
