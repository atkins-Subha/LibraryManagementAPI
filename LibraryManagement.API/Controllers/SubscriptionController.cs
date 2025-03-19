using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{

    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("GetAllSubscription")]
        public async Task<IActionResult> GetAllSubscription()
        {
            Response response = new Response();
            try
            {
                var result = await _subscriptionService.GetAllSubscriptionService();
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
        [HttpPost("CreateSubscription")]
        public async Task<IActionResult> CreateSubscription(SubscriptionRequest request)
        {
            Response response = new Response();
            if (string.IsNullOrEmpty(request.SubscriptionName))
            {
                response.statuscode = Constants.FailCode;
                response.message = "SubscriptionName is Required!";
                return Ok(response);
            }
            if (request.AnnualFee.HasValue && request.AnnualFee == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "AnnualFee is Required!";
                return Ok(response);
            }
            if (request.NoOfBooksAllowed.HasValue && request.NoOfBooksAllowed == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "CategoryId is Required!";
                return Ok(response);
            }
            if (request.NoOfDaysAllowed.HasValue && request.NoOfDaysAllowed == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "NoOfDaysAllowed is Required!";
                return Ok(response);
            }
            if (request.ValidityDays.HasValue && request.ValidityDays == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "ValidityDays is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _subscriptionService.CreateSubscriptionService(request);
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
        [HttpDelete("DeleteSubscriptionEntry")]
        public async Task<IActionResult> DeleteSubscriptionEntry(int subscriptionServiceId)
        {
            Response response = new Response();
            if (subscriptionServiceId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "subscriptionServiceId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _subscriptionService.DeleteSubscriptionService(subscriptionServiceId);
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
