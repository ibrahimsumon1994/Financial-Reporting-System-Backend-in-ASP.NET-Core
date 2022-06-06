using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IUserWiseUnitPermissionService
    {
        bool Add(UserWiseUnitPermission userWiseUnitPermission);
        List<UserWiseUnitPermission> GetAll(PagingParam param, out int total);
        bool Update(UserWiseUnitPermission userWiseUnitPermission, out string message);
        UserWiseUnitPermission GetById(long unitPermissionId);
        bool Delete(UserWiseUnitPermission userWiseUnitPermission);
        List<UserWiseUnitPermission> GetUserWiseUnitForDropdown(string userId);
    }
}
