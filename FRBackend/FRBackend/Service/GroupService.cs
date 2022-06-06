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
    public class GroupService : IGroupService
    {
        private IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public bool Add(Group group, out string message)
        {
            var checkData = _groupRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == group.NameEnglish.Trim().ToLower());
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
                group.CreatedDate = DateTime.Now;
                group.Recstatus = ActionConst.Authorize;
                _groupRepository.Add(group);
                message = MessageConst.Insert;
                return true;
            }
        }

        public List<Group> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<Group>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _groupRepository.AsQueryable()
                        .OrderByDescending(o => o.GroupId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _groupRepository.AsQueryable().Count();
            }
            else
            {
                data = _groupRepository.AsQueryable().Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()))
                        .OrderByDescending(o => o.GroupId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _groupRepository.AsQueryable().Where(x => x.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.NameBangla.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            return data;
        }

        public bool Update(Group group, out string message)
        {
            var updateGroup = _groupRepository.AsQueryable().FirstOrDefault(x => x.GroupId == group.GroupId);
            if (updateGroup != null)
            {
                if (updateGroup.NameEnglish.Trim().ToLower() == group.NameEnglish.Trim().ToLower())
                {
                    updateGroup.NameBangla = group.NameBangla;
                    updateGroup.UpdatedBy = group.UpdatedBy;
                    updateGroup.UpdatedDate = DateTime.Now;
                    _groupRepository.Update(updateGroup);
                    message = MessageConst.Update;
                    return true;
                }
                else
                {
                    var alreadyExist = _groupRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == group.NameEnglish.Trim().ToLower());
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
                        updateGroup.NameEnglish = group.NameEnglish;
                        updateGroup.NameBangla = group.NameBangla;
                        updateGroup.UpdatedBy = group.UpdatedBy;
                        updateGroup.UpdatedDate = DateTime.Now;
                        _groupRepository.Update(updateGroup);
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

        public Group GetById(long groupId)
        {
            return _groupRepository.AsQueryable().FirstOrDefault(x => x.GroupId == groupId);
        }

        public bool Delete(Group group)
        {
            var updateGroup = _groupRepository.AsQueryable().FirstOrDefault(x => x.GroupId == group.GroupId);
            if (updateGroup != null)
            {
                updateGroup.Recstatus = ActionConst.Cancel;
                updateGroup.UpdatedBy = group.UpdatedBy;
                updateGroup.UpdatedDate = DateTime.Now;
                _groupRepository.Update(updateGroup);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Restore(Group group)
        {
            var updateGroup = _groupRepository.AsQueryable().FirstOrDefault(x => x.GroupId == group.GroupId);
            if (updateGroup != null)
            {
                updateGroup.Recstatus = ActionConst.Authorize;
                updateGroup.UpdatedBy = group.UpdatedBy;
                updateGroup.UpdatedDate = DateTime.Now;
                _groupRepository.Update(updateGroup);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Group> GetAllForDropdown()
        {
            var data = new List<Group>();
            data = _groupRepository.AsQueryable().Where(x => x.Recstatus == ActionConst.Authorize)
                        .OrderBy(o => o.NameEnglish).ToList();
            return data;
        }
    }
}
