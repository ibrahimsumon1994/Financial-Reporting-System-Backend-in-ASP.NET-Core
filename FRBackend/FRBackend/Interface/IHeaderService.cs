using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IHeaderService
    {
        bool Add(Header header, out string message);
        List<HeaderModel> GetAll(PagingParam param, out int total);
        bool Update(Header header, out string message);
        Header GetById(long headerId);
        bool Delete(Header header);
        bool Restore(Header header);
        List<Header> GetAllForDropdown();
        List<Header> GetParentHeaderByTypeAndLayerForDropdown(Header header);
        string GetHeaderCode(Header header);
        List<Header> GetFirstHeaderByHeaderType(long headerTypeId);
        List<Header> GetSecondHeaderByFirstHeader(long headerId);
        List<Header> GetItemsBySecondHeader(long? headerId);
        List<Header> GetAllHeaderItemByHeaderType(long? headerTypeId);
    }
}
