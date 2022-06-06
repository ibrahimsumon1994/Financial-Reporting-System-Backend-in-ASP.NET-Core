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
    [Route("api/Department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        ApiReturnObj returnObj = new ApiReturnObj();
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult Add(Department department)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                department.CreatedBy = Convert.ToInt32(loginUser);
                var data = _departmentService.Add(department, out string message);
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
                var data = _departmentService.GetAll(param, out int total);
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
        public IActionResult Update(Department department)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                department.UpdatedBy = Convert.ToInt32(loginUser);
                var data = _departmentService.Update(department, out string message);
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
        public IActionResult GetById(long departmentId)
        {
            try
            {
                var data = _departmentService.GetById(departmentId);
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
        [Route("Delete/{departmentId}")]
        [Authorize]
        public IActionResult Delete(long departmentId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var department = new Department();
                department.UpdatedBy = Convert.ToInt32(loginUser);
                department.DepartmentId = departmentId;
                var data = _departmentService.Delete(department);
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
        [Route("Restore/{departmentId}")]
        [Authorize]
        public IActionResult Restore(long departmentId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var department = new Department();
                department.UpdatedBy = Convert.ToInt32(loginUser);
                department.DepartmentId = departmentId;
                var data = _departmentService.Restore(department);
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
                var data = _departmentService.GetAllForDropdown();
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

        [HttpGet]
        [Route("GetByUnitForDropdown/{unitId}")]
        [Authorize]
        public IActionResult GetByUnitForDropdown(long unitId)
        {
            try
            {
                var data = _departmentService.GetByUnitForDropdown(unitId);
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
