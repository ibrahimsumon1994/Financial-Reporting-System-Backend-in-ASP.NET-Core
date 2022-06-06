using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IDepartmentService
    {
        bool Add(Department department, out string message);
        List<Department> GetAll(PagingParam param, out int total);
        bool Update(Department department, out string message);
        Department GetById(long unitId);
        bool Delete(Department department);
        bool Restore(Department department);
        List<Department> GetAllForDropdown();
        List<Department> GetByUnitForDropdown(long unitId);
    }
}
