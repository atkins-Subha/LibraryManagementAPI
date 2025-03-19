using Azure.Core;
using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{

    [ApiController]
    [Route("api/subscriptiondetails")]
    public class SubscriptionDetailsController : ControllerBase
    {
        private readonly ISubscriptionDetailsService _subscriptionDetailsService;

        public SubscriptionDetailsController(ISubscriptionDetailsService subscriptionDetailsService)
        {
            _subscriptionDetailsService = subscriptionDetailsService;
        }

        [HttpGet("GetAllSubscriptionDetails")]
        public async Task<IActionResult> GetAllSubscriptionDetails()
        {
            Response response = new Response();
            try
            {
                var result = await _subscriptionDetailsService.GetAllSubscriptionDetailsService();
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
        [HttpGet("GetSubscriptionDetailsByUser")]
        public async Task<IActionResult> GetSubscriptionDetailsByUser(int userId)
        {
            Response response = new Response();
            try
            {
                var result =_subscriptionDetailsService.GetAllSubscriptionDetailsService().Result.FirstOrDefault(x => x.UserId == userId);

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
        [HttpGet("GetSubscriptionDetailsbyId")]
        public async Task<IActionResult> GetSubscriptionDetailsbyId([FromQuery] int subscriptionDetailId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (subscriptionDetailId <= 0)
                {
                    return BadRequest("Invalid Book Datail Id.");
                }

                var result = await _subscriptionDetailsService.GetSubscriptionDetailsByIdService(subscriptionDetailId);

                if (result == null)
                {
                    return NotFound("subscription Datail not found.");
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
        [HttpPost("CreateSubscriptionDetails")]
        public async Task<IActionResult> CreateSubscriptionDetails(SubscriptionDetailsRequest request)
        {
            Response response = new Response();
            if (request.UserId.HasValue && request.UserId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserId is Required!";
                return Ok(response);
            }
            if (request.SubscriptionId.HasValue && request.SubscriptionId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "SubscriptionId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _subscriptionDetailsService.CreateSubscriptionDetailsService(request);
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
        [HttpDelete("DeleteSubscriptionDetails")]
        public async Task<IActionResult> DeleteSubscriptionDetails(int subscriptionDetailsId)
        {
            Response response = new Response();
            if (subscriptionDetailsId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "subscriptionDetailsId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _subscriptionDetailsService.DeleteSubscriptionDetailsService(subscriptionDetailsId);
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
