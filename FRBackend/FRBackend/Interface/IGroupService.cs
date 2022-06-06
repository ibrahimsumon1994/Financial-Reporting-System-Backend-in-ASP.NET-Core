using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IGroupService
    {
        bool Add(Group group, out string message);
        List<Group> GetAll(PagingParam param, out int total);
        bool Update(Group group, out string message);
        Group GetById(long groupId);
        bool Delete(Group group);
        bool Restore(Group group);
        List<Group> GetAllForDropdown();
    }
}
