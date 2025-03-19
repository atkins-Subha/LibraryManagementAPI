using Azure.Core;
using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{

    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            Response response = new Response();
            try
            {
                var result = await _roleService.GetAllRolesService();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
        }
        //[Authorize(Roles = "Librarian")]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleRequest request)
        {
            Response response = new Response();

            if (string.IsNullOrEmpty(request.RoleName))
            {
                response.statuscode = Constants.FailCode;
                response.message = "RoleName is Required!";
                return Ok(response);
            }

            try
            {
                var result = await _roleService.CreateRoleService(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }

        }
        //[Authorize(Roles = "Librarian")]
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            Response response = new Response();
            if (roleId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "roleId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _roleService.DeleteRoleService(roleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
        }

    }
}
