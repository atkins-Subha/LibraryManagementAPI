using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }


        [HttpGet("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            Response response = new Response();
            try
            {
                var result = await _statusService.GetAllStatusService();
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
        [HttpPost("CreateStatus")]
        public async Task<IActionResult> CreateStatus(StatusRequest request)
        {
            Response response = new Response();

            if (string.IsNullOrEmpty(request.StatusName))
            {
                response.statuscode = Constants.FailCode;
                response.message = "StatusName is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _statusService.CreateStatusService(request);
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
        [HttpDelete("DeleteStatus")]
        public async Task<IActionResult> DeleteStatus(int statusId)
        {
            Response response = new Response();
            if (statusId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "StatusId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _statusService.DeleteStatusService(statusId);
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
