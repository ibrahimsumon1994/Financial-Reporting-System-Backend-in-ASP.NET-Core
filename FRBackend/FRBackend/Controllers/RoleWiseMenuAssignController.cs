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
    [Route("api/RoleWiseMenuAssign")]
    [ApiController]
    public class RoleWiseMenuAssignController : ControllerBase
    {
        ApiReturnObj returnObj = new ApiReturnObj();
        private readonly IRoleWiseMenuAssignService _roleWiseMenuAssignService;

        public RoleWiseMenuAssignController(IRoleWiseMenuAssignService roleWiseMenuAssignService)
        {
            _roleWiseMenuAssignService = roleWiseMenuAssignService;
        }

        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult Add(RoleWiseMenuAssign roleWiseMenuAssign)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                roleWiseMenuAssign.CreatedBy = Convert.ToInt32(loginUser);
                var data = _roleWiseMenuAssignService.Add(roleWiseMenuAssign);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = MessageConst.Insert;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.ApiData = null;
                    returnObj.Message = MessageConst.IsExist;
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
        [Route("AddList")]
        [Authorize]
        public IActionResult AddList(List<RoleWiseMenuAssign> roleWiseMenuAssignList)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var data = _roleWiseMenuAssignService.AddList(roleWiseMenuAssignList, loginUser, out string message);
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
        [Route("GetAll")]
        [Authorize]
        public IActionResult GetAll(PagingParam param)
        {
            try
            {
                var data = _roleWiseMenuAssignService.GetAll(param, out int total);
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
        public IActionResult Update(RoleWiseMenuAssign roleWiseMenuAssign)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                roleWiseMenuAssign.UpdatedBy = Convert.ToInt32(loginUser);
                var data = _roleWiseMenuAssignService.Update(roleWiseMenuAssign, out string message);
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
        public IActionResult GetById(long roleWiseMenuAssignId)
        {
            try
            {
                var data = _roleWiseMenuAssignService.GetById(roleWiseMenuAssignId);
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
        [Route("Delete/{roleWiseMenuAssignId}")]
        [Authorize]
        public IActionResult Delete(long roleWiseMenuAssignId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var roleWiseMenuAssign = new RoleWiseMenuAssign();
                roleWiseMenuAssign.UpdatedBy = Convert.ToInt32(loginUser);
                roleWiseMenuAssign.RoleWiseMenuAssignId = roleWiseMenuAssignId;
                var data = _roleWiseMenuAssignService.Delete(roleWiseMenuAssign);
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
    }
}
