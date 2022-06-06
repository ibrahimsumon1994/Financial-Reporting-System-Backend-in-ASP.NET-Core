using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IRoleService
    {
        bool Add(Role role, out string message);
        List<Role> GetAll(PagingParam param, out int total);
        bool Update(Role role, out string message);
        Role GetById(long roleId);
        bool Delete(Role role);
        bool Restore(Role role);
        List<Role> GetAllForDropdown();
    }
}
