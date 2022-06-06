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
    [Route("api/Designation")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        ApiReturnObj returnObj = new ApiReturnObj();
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult Add(Designation designation)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                designation.CreatedBy = Convert.ToInt32(loginUser);
                var data = _designationService.Add(designation, out string message);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = message;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.ApiData = null;
                    returnObj.Message = message;
                    return Ok(returnObj);
                }
            }
            catch(Exception ex)
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

        [HttpPost]
        [Route("GetAll")]
        [Authorize]
        public IActionResult GetAll(PagingParam param)
        {
            try
            {
                var data = _designationService.GetAll(param, out int total);
                if (data.Any())
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.TotalRecord = total;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.Message = MessageConst.NotFound;
                    returnObj.ApiData = null;
                    return Ok(returnObj);
                }
            }
            catch (Exception ex)
            {
                returnObj.IsExecute = false;
                returnObj.Message = ex.Message;
                returnObj.ApiData = null;
                return Ok(returnObj);
            }
        }

        [HttpPost]
        [Route("Update")]
        [Authorize]
        public IActionResult Update(Designation designation)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                designation.UpdatedBy = Convert.ToInt32(loginUser);
                var data = _designationService.Update(designation, out string message);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = message;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.ApiData = null;
                    returnObj.Message = message;
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

        [HttpPost]
        [Route("GetById")]
        [Authorize]
        public IActionResult GetById(long designationId)
        {
            try
            {
                var data = _designationService.GetById(designationId);
                if (data != null)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = MessageConst.IsExist;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.Message = MessageConst.NotFound;
                    returnObj.ApiData = null;
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

        [HttpGet]
        [Route("Delete/{designationId}")]
        [Authorize]
        public IActionResult Delete(long designationId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var designation = new Designation();
                designation.UpdatedBy = Convert.ToInt32(loginUser);
                designation.DesignationId = designationId;
                var data = _designationService.Delete(designation);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = MessageConst.Active;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.Message = MessageConst.Failed;
                    returnObj.ApiData = null;
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

        [HttpGet]
        [Route("Restore/{designationId}")]
        [Authorize]
        public IActionResult Restore(long designationId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var designation = new Designation();
                designation.UpdatedBy = Convert.ToInt32(loginUser);
                designation.DesignationId = designationId;
                var data = _designationService.Restore(designation);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = MessageConst.Deactive;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.Message = MessageConst.Failed;
                    returnObj.ApiData = null;
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

        [HttpGet]
        [Route("GetAllForDropdown")]
        [Authorize]
        public IActionResult GetAllForDropdown()
        {
            try
            {
                var data = _designationService.GetAllForDropdown();
                if (data.Any())
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.Message = MessageConst.NotFound;
                    returnObj.ApiData = null;
                    return Ok(returnObj);
                }
            }
            catch (Exception ex)
            {
                returnObj.IsExecute = false;
                returnObj.Message = ex.Message;
                returnObj.ApiData = null;
                return Ok(returnObj);
            }
        }
    }
}
