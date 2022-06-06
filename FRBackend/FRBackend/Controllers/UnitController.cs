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
    [Route("api/Unit")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        ApiReturnObj returnObj = new ApiReturnObj();
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult Add(Unit unit)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                unit.CreatedBy = Convert.ToInt32(loginUser);
                var data = _unitService.Add(unit, out string message);
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
                var data = _unitService.GetAll(param, out int total);
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
        public IActionResult Update(Unit unit)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                unit.UpdatedBy = Convert.ToInt32(loginUser);
                var data = _unitService.Update(unit, out string message);
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
        public IActionResult GetById(long unitId)
        {
            try
            {
                var data = _unitService.GetById(unitId);
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
        [Route("Delete/{unitId}")]
        [Authorize]
        public IActionResult Delete(long unitId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var unit = new Unit();
                unit.UpdatedBy = Convert.ToInt32(loginUser);
                unit.UnitId = unitId;
                var data = _unitService.Delete(unit);
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
        [Route("Restore/{unitId}")]
        [Authorize]
        public IActionResult Restore(long unitId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var unit = new Unit();
                unit.UpdatedBy = Convert.ToInt32(loginUser);
                unit.UnitId = unitId;
                var data = _unitService.Restore(unit);
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
                var data = _unitService.GetAllForDropdown();
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
        [Route("GetByGroupForDropdown/{groupId}")]
        [Authorize]
        public IActionResult GetByGroupForDropdown(long groupId)
        {
            try
            {
                var data = _unitService.GetByGroupForDropdown(groupId);
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
