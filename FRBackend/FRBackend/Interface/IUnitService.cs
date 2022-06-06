using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IUnitService
    {
        bool Add(Unit unit, out string message);
        List<Unit> GetAll(PagingParam param, out int total);
        bool Update(Unit unit, out string message);
        Unit GetById(long unitId);
        bool Delete(Unit unit);
        bool Restore(Unit unit);
        List<Unit> GetAllForDropdown();
        List<Unit> GetByGroupForDropdown(long groupId);
    }
}
