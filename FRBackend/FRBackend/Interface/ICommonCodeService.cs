using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface ICommonCodeService
    {
        bool Add(CommonCode commonCode, out string message);
        List<CommonCode> GetAll(PagingParam param, out int total);
        bool Update(CommonCode commonCode, out string message);
        CommonCode GetById(long commonCodeId);
        bool Delete(CommonCode commonCode);
        bool Restore(CommonCode commonCode);
        List<CommonCode> GetByType(string type);
    }
}
