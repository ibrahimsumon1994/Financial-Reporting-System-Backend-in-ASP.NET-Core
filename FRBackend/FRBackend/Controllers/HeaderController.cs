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
    [Route("api/Header")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        ApiReturnObj returnObj = new ApiReturnObj();
        private readonly IHeaderService _headerService;

        public HeaderController(IHeaderService headerService)
        {
            _headerService = headerService;
        }

        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult Add(Header header)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                header.CreatedBy = Convert.ToInt32(loginUser);
                var data = _headerService.Add(header, out string message);
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
                var data = _headerService.GetAll(param, out int total);
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
        public IActionResult Update(Header header)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                header.UpdatedBy = Convert.ToInt32(loginUser);
                var data = _headerService.Update(header, out string message);
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
        public IActionResult GetById(long headerId)
        {
            try
            {
                var data = _headerService.GetById(headerId);
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
        [Route("Delete/{headerId}")]
        [Authorize]
        public IActionResult Delete(long headerId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var header = new Header();
                header.UpdatedBy = Convert.ToInt32(loginUser);
                header.HeaderId = headerId;
                var data = _headerService.Delete(header);
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
        [Route("Restore/{headerId}")]
        [Authorize]
        public IActionResult Restore(long headerId)
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loginUser = claim.Value;
                var header = new Header();
                header.UpdatedBy = Convert.ToInt32(loginUser);
                header.HeaderId = headerId;
                var data = _headerService.Restore(header);
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
        [Route("GetAllForDropdown")]
        [Authorize]
        public IActionResult GetAllForDropdown()
        {
            try
            {
                var data = _headerService.GetAllForDropdown();
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

        [HttpPost]
        [Route("GetParentHeaderByTypeAndLayerForDropdown")]
        [Authorize]
        public IActionResult GetParentHeaderByTypeAndLayerForDropdown(Header header)
        {
            try
            {
                var data = _headerService.GetParentHeaderByTypeAndLayerForDropdown(header);
                if (data.Any())
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
                returnObj.Message = ex.Message;
                returnObj.ApiData = null;
                return Ok(returnObj);
            }
        }

        [HttpPost]
        [Route("GetHeaderCode")]
        [Authorize]
        public IActionResult GetHeaderCode(Header header)
        {
            try
            {
                var data = _headerService.GetHeaderCode(header);
                if (!string.IsNullOrEmpty(data))
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
                returnObj.Message = ex.Message;
                returnObj.ApiData = null;
                return Ok(returnObj);
            }
        }

        [HttpGet]
        [Route("GetFirstHeaderByHeaderType/{headerTypeId}")]
        [Authorize]
        public IActionResult GetFirstHeaderByHeaderType(long headerTypeId)
        {
            try
            {
                var data = _headerService.GetFirstHeaderByHeaderType(headerTypeId);
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
        [Route("GetSecondHeaderByFirstHeader/{headerId}")]
        [Authorize]
        public IActionResult GetSecondHeaderByFirstHeader(long headerId)
        {
            try
            {
                var data = _headerService.GetSecondHeaderByFirstHeader(headerId);
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
