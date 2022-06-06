using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IRoleWiseMenuAssignService
    {
        bool Add(RoleWiseMenuAssign roleWiseMenuAssign);
        bool AddList(List<RoleWiseMenuAssign> roleWiseMenuAssignList, string loginUser, out string message);
        List<RoleWiseMenuAssign> GetAll(PagingParam param, out int total);
        bool Update(RoleWiseMenuAssign roleWiseMenuAssign, out string message);
        RoleWiseMenuAssign GetById(long roleWiseMenuAssignId);
        bool Delete(RoleWiseMenuAssign roleWiseMenuAssign);
    }
}
