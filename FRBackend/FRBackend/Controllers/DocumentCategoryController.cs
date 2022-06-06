using Common.Helpers;
using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Helpers;
using FRBackend.Interface;
using FRBackend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;




namespace FRBackend.Controllers
{
    [Route("api/DocumentCategory")]
    [ApiController]
    public class DocumentCategoryController : ControllerBase
    {
        ApiReturnObj returnObj = new ApiReturnObj();
        private readonly IDocumentCategoryService _documentCategoryService;

        public DocumentCategoryController(IDocumentCategoryService documentCategoryService)
        {
            _documentCategoryService = documentCategoryService;
        }

        [HttpPost]
        [Route("Add")]
        //[Authorize]
        public IActionResult Add(DocumentCategory documentCategory)
        {
            try
            {
                //var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //var loginUser = claim.Value;
                //documentCategory.CreatedBy = Convert.ToInt32(loginUser);
                //documentCategory.UpdatedBy = Convert.ToInt32("");
                var data = _documentCategoryService.Add(documentCategory);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    //returnObj.Message = message;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.ApiData = null;
                    documentCategory.CreatedDate = System.DateTime.Now;
                    documentCategory.UpdatedDate = System.DateTime.Now;
                    //returnObj.Message = message;
                    return Ok(returnObj);
                }
            }
            catch (Exception ex)
            {
                returnObj.IsExecute = false;
                returnObj.ApiData = null;
                if (ex.InnerException != null)
                {
                    returnObj.Message = ex.InnerException.Message;
                }
                else
                {
                    returnObj.Message = ex.Message;
                }
                return Ok(returnObj);
            }
        }


    }
}
