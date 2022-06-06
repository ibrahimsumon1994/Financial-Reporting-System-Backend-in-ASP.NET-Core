using Common.Helpers;
using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Helpers;
using FRBackend.Interface;
using FRBackend.Model;
using FRBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Service
{
    public class DocumentCategoryService : IDocumentCategoryService
    {
        private IDocumentCategoryRepository _documentCategoryRepository;
        public DocumentCategoryService(IDocumentCategoryRepository documentCategoryRepository)
        {
            _documentCategoryRepository = documentCategoryRepository;
        }

        public bool Add(DocumentCategory documentCategory)
        {
            _documentCategoryRepository.Add(documentCategory);
            return true;
        }
    }
}
