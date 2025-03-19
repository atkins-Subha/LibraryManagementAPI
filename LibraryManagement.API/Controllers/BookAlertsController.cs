using Azure.Core;
using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using System.Runtime.InteropServices;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/bookalerts")]
    public class BookAlertsController : ControllerBase
    {
        private readonly IBookAlertsService _bookAlertsService;

        public BookAlertsController(IBookAlertsService bookAlertsService)
        {
            _bookAlertsService = bookAlertsService;
        }

        [HttpGet("GetAllBookAlerts")]
        public async Task<IActionResult> GetAllBookAlerts()
        {
            Response response = new Response();
            try
            {
                var result = await _bookAlertsService.GetAllBookAlertsService();
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


        [HttpGet("GetBookAlertsbyId")]
        public async Task<IActionResult> GetBookAlertsbyId([FromQuery] int bookAlertsId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (bookAlertsId <= 0)
                {
                    return BadRequest("Invalid Book Alerts Id.");
                }

                var result = await _bookAlertsService.GetBookAlertsByIdService(bookAlertsId);

                if (result == null)
                {
                    return NotFound("Book Datail not found.");
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
        [HttpPost("CreateBookAlerts")]
        public async Task<IActionResult> CreateBookAlerts(BookAlertsRequest request)
        {
            Response response = new Response();
            if (request.BookId.HasValue && request.BookId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookId is Required!";
                return Ok(response);
            }
            if (request.UserId.HasValue && request.UserId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookAlertsService.CreateBookAlertsService(request);

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
        [HttpPut("UpdateBookAlerts")]
        public async Task<IActionResult> UpdateBookAlerts([FromBody] BookAlertsRequest request)
        {
            Response response = new Response();
            try
            {
                if (request.BookAlertsId.HasValue && request.BookAlertsId == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = "BookAlertsId is Required!";
                    return Ok(response);
                }

                var result = await _bookAlertsService.UpdateBookAlertsService(request);
                //var result = "";
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
        [HttpDelete("DeleteBookAlerts")]
        public async Task<IActionResult> DeleteBookAlerts(int bookAlertsId)
        {
            Response response = new Response();
            if (bookAlertsId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookAlertsId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookAlertsService.DeleteBookAlertsService(bookAlertsId);
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
