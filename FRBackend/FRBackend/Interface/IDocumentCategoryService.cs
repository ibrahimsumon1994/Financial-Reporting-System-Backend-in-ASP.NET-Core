using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IDocumentCategoryService
    {
        bool Add(DocumentCategory documentCategory);
        //List<Designation> GetAll(PagingParam param, out int total);
        //bool Update(DocumentCategory documentCategory, out string message);
        //Designation GetById(long DocumentCategoryId);
        //bool Delete(DocumentCategory documentCategory);
        //bool Restore(DocumentCategory documentCategory);
        //List<DocumentCategory> GetAllForDropdown();
    }
}
