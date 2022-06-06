using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IRoleAssignService
    {
        bool Add(RoleAssign roleAssign);
        List<RoleAssign> GetAll(PagingParam param, out int total);
        bool Update(RoleAssign roleAssign, out string message);
        RoleAssign GetById(long roleAssignId);
        bool Delete(RoleAssign roleAssign);
    }
}
