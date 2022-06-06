using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IDesignationService
    {
        bool Add(Designation designation, out string message);
        List<Designation> GetAll(PagingParam param, out int total);
        bool Update(Designation designation, out string message);
        Designation GetById(long designationId);
        bool Delete(Designation designation);
        bool Restore(Designation designation);
        List<Designation> GetAllForDropdown();
    }
}
