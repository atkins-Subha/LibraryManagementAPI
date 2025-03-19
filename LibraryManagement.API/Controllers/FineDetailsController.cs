using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/FineDetails")]
    public class FineDetailsController : ControllerBase
    {
        private readonly IFineDetailsService _FineDetailsService;

        public FineDetailsController(IFineDetailsService FineDetailsService)
        {
            _FineDetailsService = FineDetailsService;
        }

        [HttpGet("GetAllFineDetails")]
        public async Task<IActionResult> GetAllFineDetails()
        {
            Response response = new Response();
            try
            {
                var result = await _FineDetailsService.GetAllFineDetailssService();
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

        [HttpGet("GetFineDetailsbyId")]
        public async Task<IActionResult> GetFineDetailsbyId([FromQuery] int fineDetailId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (fineDetailId <= 0)
                {
                    return BadRequest("Invalid fine Datail Id.");
                }

                var result = await _FineDetailsService.GetFineDetailsByIdService(fineDetailId);

                if (result == null)
                {
                    return NotFound("Fine Datail not found.");
                }
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
        [HttpPost("CreateFineDetails")]
        public async Task<IActionResult> CreateFineDetails(FineDetailsRequest request)
        {
            Response response = new Response();
            if (request.FineId.HasValue && request.FineId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "FineId is Required!";
                return Ok(response);
            }
            if (request.UserId.HasValue && request.UserId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserId is Required!";
                return Ok(response);
            }
            if (request.BookId.HasValue && request.BookId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookId is Required!";
                return Ok(response);
            }
            if (request.DueDate.HasValue && request.FineDate == default(DateTime))
            {
                response.statuscode = Constants.FailCode;
                response.message = "FineDate is Required!";
                return Ok(response);
            }
            if (string.IsNullOrEmpty(request.Reason))
            {
                response.statuscode = Constants.FailCode;
                response.message = "Reason is Required!";
                return Ok(response);
            }

            try
            {
                var result = await _FineDetailsService.CreateFineDetailsService(request);
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
        [HttpDelete("DeleteFineDetails")]
        public async Task<IActionResult> DeleteFineDetails(int FineDetailsId)
        {
            Response response = new Response();
            if (FineDetailsId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "FineDetailsId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _FineDetailsService.DeleteFineDetailsService(FineDetailsId);
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
