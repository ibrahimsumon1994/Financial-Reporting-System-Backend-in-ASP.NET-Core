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
    public class HeaderService : IHeaderService
    {
        private IHeaderRepository _headerRepository;
        public HeaderService(IHeaderRepository headerRepository)
        {
            _headerRepository = headerRepository;
        }

        public bool Add(Header header, out string message)
        {
            if (header.HeaderLayer == HeaderLayerConst.FirstLayer)
            {
                var checkData = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderName.Trim().ToLower() == header.HeaderName.Trim().ToLower()
                                && x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == header.HeaderLayer && x.Recstatus == ActionConst.Authorize);
                if (checkData != null)
                {
                    message = MessageConst.IsExist;
                    return false;
                }
            }
            else
            {
                var checkData = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderName.Trim().ToLower() == header.HeaderName.Trim().ToLower()
                                && x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == header.HeaderLayer && x.ParentHeaderId == header.ParentHeaderId
                                && x.Recstatus == ActionConst.Authorize);
                if (checkData != null)
                {
                    message = MessageConst.IsExist;
                    return false;
                }
            }
            header.CreatedDate = DateTime.Now;
            header.Recstatus = ActionConst.Authorize;
            _headerRepository.Add(header);
            message = MessageConst.Insert;
            return true;
        }

        public List<HeaderModel> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<HeaderModel>();
            var headers = new List<Header>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                headers = _headerRepository.AsQueryable().Include(x => x.HeaderType).Where(x => x.Recstatus == ActionConst.Authorize)
                            .OrderBy(o => o.HeaderTypeId).ThenBy(x => x.HeaderCode.Length).ThenBy(x => x.HeaderCode).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();

                total = _headerRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).Count();
            }
            else
            {
                headers = _headerRepository.AsQueryable().Include(x => x.HeaderType).Where(x => x.Recstatus == ActionConst.Authorize && (x.HeaderName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.HeaderCode.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.HeaderType.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())))
                        .OrderBy(o => o.HeaderTypeId).ThenBy(x => x.HeaderCode.Length).ThenBy(x => x.HeaderCode).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _headerRepository.AsQueryable().Include(x => x.HeaderType).Where(x => x.Recstatus == ActionConst.Authorize && (x.HeaderName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.HeaderCode.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) || x.HeaderType.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))).Count();
            }
            foreach (var header in headers)
            {
                var obj = new HeaderModel();
                obj.HeaderId = header.HeaderId;
                obj.HeaderName = header.HeaderName;
                obj.HeaderCode = header.HeaderCode;
                if (header.HeaderType != null)
                {
                    obj.HeaderTypeId = header.HeaderTypeId;
                    obj.HeaderType = header.HeaderType.NameEnglish;
                }
                obj.HeaderLayer = header.HeaderLayer;
                var parentData = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == header.ParentHeaderId);
                if (parentData != null)
                {
                    obj.ParentHeaderId = parentData.HeaderId;
                    obj.ParentHeader = parentData.HeaderName;
                }
                obj.Recstatus = header.Recstatus;
                obj.Remarks = header.Remarks;
                data.Add(obj);
            }
            return data;
        }

        public bool Update(Header header, out string message)
        {
            var updateHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == header.HeaderId);
            if (updateHeader != null)
            {
                if (updateHeader.HeaderName.Trim().ToLower() == header.HeaderName.Trim().ToLower() && updateHeader.HeaderTypeId == header.HeaderTypeId && updateHeader.HeaderLayer == header.HeaderLayer && updateHeader.ParentHeaderId == header.ParentHeaderId)
                {
                    updateHeader.Remarks = header.Remarks;
                    updateHeader.UpdatedBy = header.UpdatedBy;
                    updateHeader.UpdatedDate = DateTime.Now;
                    _headerRepository.Update(updateHeader);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    if (header.HeaderLayer == HeaderLayerConst.FirstLayer)
                    {
                        var checkData = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderName.Trim().ToLower() == header.HeaderName.Trim().ToLower()
                                        && x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == header.HeaderLayer && x.Recstatus == ActionConst.Authorize);
                        if (checkData != null)
                        {
                            message = MessageConst.IsExist;
                            return false;
                        }
                    }
                    else
                    {
                        var checkData = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderName.Trim().ToLower() == header.HeaderName.Trim().ToLower()
                                        && x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == header.HeaderLayer && x.ParentHeaderId == header.ParentHeaderId
                                        && x.Recstatus == ActionConst.Authorize);
                        if (checkData != null)
                        {
                            message = MessageConst.IsExist;
                            return false;
                        }
                    }
                    updateHeader.HeaderName = header.HeaderName;
                    updateHeader.HeaderTypeId = header.HeaderTypeId;
                    updateHeader.HeaderLayer = header.HeaderLayer;
                    updateHeader.ParentHeaderId = header.ParentHeaderId;
                    updateHeader.HeaderCode = header.HeaderCode;
                    updateHeader.CreatedDate = DateTime.Now;
                    updateHeader.Recstatus = ActionConst.Authorize;
                    _headerRepository.Update(updateHeader);
                    message = MessageConst.Update;
                    return true;
                }
            }
            else
            {
                message = MessageConst.NotFound;
                return false;
            }
        }

        public Header GetById(long headerId)
        {
            return _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == headerId);
        }

        public bool Delete(Header header)
        {
            var updateHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == header.HeaderId);
            if (updateHeader != null)
            {
                updateHeader.Recstatus = ActionConst.Cancel;
                updateHeader.UpdatedBy = header.UpdatedBy;
                updateHeader.UpdatedDate = DateTime.Now;
                _headerRepository.Update(updateHeader);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(Header header)
        {
            var updateHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == header.HeaderId);
            if (updateHeader != null)
            {
                updateHeader.Recstatus = ActionConst.Authorize;
                updateHeader.UpdatedBy = header.UpdatedBy;
                updateHeader.UpdatedDate = DateTime.Now;
                _headerRepository.Update(updateHeader);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Header> GetAllForDropdown()
        {
            var data = new List<Header>();
            data = _headerRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize).Select(x => new Header()
                    {
                        HeaderId = x.HeaderId,
                        HeaderName = x.HeaderName
                    })
                    .OrderBy(o => o.HeaderName).ToList();
            return data;
        }

        public List<Header> GetParentHeaderByTypeAndLayerForDropdown(Header header)
        {
            var data = new List<Header>();
            if (header.HeaderLayer == HeaderLayerConst.SecondLayer)
            {
                data = _headerRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize && x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == HeaderLayerConst.FirstLayer).Select(x => new Header()
                {
                    HeaderId = x.HeaderId,
                    HeaderName = x.HeaderName
                    //HeaderName = x.HeaderName + (x.HeaderLayer == 1 ? " (1st Layer)" : (x.HeaderLayer == 2 ? " (2nd Layer)" : (x.HeaderLayer == 3 ? " (3rd Layer)" : "")))
                }).OrderBy(o => o.HeaderName).ToList();
            }
            else if (header.HeaderLayer == HeaderLayerConst.ThirdLayer)
            {
                data = _headerRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize && x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == HeaderLayerConst.SecondLayer).Select(x => new Header()
                {
                    HeaderId = x.HeaderId,
                    HeaderName = x.HeaderName
                    //HeaderName = x.HeaderName + (x.HeaderLayer == 1 ? " (1st Layer)" : (x.HeaderLayer == 2 ? " (2nd Layer)" : (x.HeaderLayer == 3 ? " (3rd Layer)" : "")))
                }).OrderBy(o => o.HeaderName).ToList();
            }
            return data;
        }

        public string GetHeaderCode(Header header)
        {
            string headerCode = "";
            string parentHeaderCode = "";
            if (header.HeaderLayer != HeaderLayerConst.FirstLayer)
            {
                var parentHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == header.ParentHeaderId);
                if (parentHeader != null)
                {
                    //string[] authorInfo = parentHeader.HeaderCode.Split(".");
                    //parentHeaderCode = authorInfo[0];
                    parentHeaderCode = parentHeader.HeaderCode;
                }
            }
            if (header.HeaderLayer == HeaderLayerConst.FirstLayer)
            {
                var data = _headerRepository.AsQueryable().Where(x => x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == HeaderLayerConst.FirstLayer && x.Recstatus == ActionConst.Authorize).OrderBy(x => x.HeaderCode).LastOrDefault();
                if (data != null)
                {
                    headerCode = (Convert.ToInt32(data.HeaderCode) + 10).ToString();
                }
                else
                {
                    headerCode = "10";
                }
            }
            else if (header.HeaderLayer == HeaderLayerConst.SecondLayer)
            {
                if (!string.IsNullOrEmpty(parentHeaderCode))
                {
                    var data = _headerRepository.AsQueryable().Where(x => x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == HeaderLayerConst.SecondLayer && x.ParentHeaderId == header.ParentHeaderId && x.Recstatus == ActionConst.Authorize).OrderBy(x => x.HeaderCode).LastOrDefault();
                    if (data != null)
                    {
                        string lastDigits = data.HeaderCode.Substring(data.HeaderCode.Length - 3);
                        headerCode = parentHeaderCode + "." + (Convert.ToInt32(lastDigits) + 1).ToString("D3");
                    }
                    else
                    {
                        headerCode = parentHeaderCode + ".001";
                    }
                }
            }
            else if (header.HeaderLayer == HeaderLayerConst.ThirdLayer)
            {
                if (!string.IsNullOrEmpty(parentHeaderCode))
                {
                    var data = _headerRepository.AsQueryable().Where(x => x.HeaderTypeId == header.HeaderTypeId && x.HeaderLayer == HeaderLayerConst.ThirdLayer && x.ParentHeaderId == header.ParentHeaderId && x.Recstatus == ActionConst.Authorize).OrderBy(x => x.HeaderCode).LastOrDefault();
                    if (data != null)
                    {
                        string lastDigits = data.HeaderCode.Substring(data.HeaderCode.Length - 4);
                        headerCode = parentHeaderCode + "." + (Convert.ToInt32(lastDigits) + 1).ToString("D4");
                    }
                    else
                    {
                        headerCode = parentHeaderCode + ".0001";
                    }
                }
            }
            return headerCode;
        }

        public List<Header> GetFirstHeaderByHeaderType(long headerTypeId)
        {
            var data = new List<Header>();
            data = _headerRepository.AsQueryable().Where(x => x.HeaderTypeId == headerTypeId && x.HeaderLayer == HeaderLayerConst.FirstLayer && x.Recstatus == ActionConst.Authorize).Select(x => new Header()
            {
                HeaderId = x.HeaderId,
                HeaderName = x.HeaderName
            })
                    .OrderBy(o => o.HeaderName).ToList();
            return data;
        }

        public List<Header> GetSecondHeaderByFirstHeader(long headerId)
        {
            var data = new List<Header>();
            data = _headerRepository.AsQueryable().Where(x => x.ParentHeaderId == headerId && x.HeaderLayer == HeaderLayerConst.SecondLayer && x.Recstatus == ActionConst.Authorize).Select(x => new Header()
            {
                HeaderId = x.HeaderId,
                HeaderName = x.HeaderName
            })
                    .OrderBy(o => o.HeaderName).ToList();
            return data;
        }

        public List<Header> GetItemsBySecondHeader(long? headerId)
        {
            var data = new List<Header>();
            data = _headerRepository.AsQueryable().Where(x => x.ParentHeaderId == headerId && x.HeaderLayer == HeaderLayerConst.ThirdLayer && x.Recstatus == ActionConst.Authorize).Select(x => new Header()
            {
                HeaderId = x.HeaderId,
                HeaderName = x.HeaderName,
                HeaderCode = x.HeaderCode
            })
                    .OrderBy(o => o.HeaderCode).ToList();
            return data;
        }

        public List<Header> GetAllHeaderItemByHeaderType(long? headerTypeId)
        {
            var data = new List<Header>();
            data = _headerRepository.AsQueryable().Where(x => x.HeaderTypeId == headerTypeId && x.HeaderLayer == HeaderLayerConst.ThirdLayer && x.Recstatus == ActionConst.Authorize)
            .OrderBy(o => o.HeaderCode).ToList();
            return data;
        }
    }
}
