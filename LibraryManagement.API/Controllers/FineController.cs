using Azure.Core;
using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/Fine")]
    public class FineController : ControllerBase
    {
        private readonly IFineService _FineService;

        public FineController(IFineService FineService)
        {
            _FineService = FineService;
        }

        [HttpGet("GetAllFine")]
        public async Task<IActionResult> GetAllFine()
        {
            Response response = new Response();
            try
            {
                var result = await _FineService.GetAllFinesService();
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
        [HttpPost("CreateFine")]
        public async Task<IActionResult> CreateFine(FineRequest request)
        {
            Response response = new Response();
            if (string.IsNullOrEmpty(request.DaysRange))
            {
                response.statuscode = Constants.FailCode;
                response.message = "DaysRange is Required!";
                return Ok(response);
            }
            if (request.FineAmount.HasValue && request.FineAmount == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "FineAmount is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _FineService.CreateFineService(request);
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
        [HttpDelete("DeleteFine")]
        public async Task<IActionResult> DeleteFine(int FineId)
        {
            Response response = new Response();
            if (FineId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "FineId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _FineService.DeleteFineService(FineId);
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
