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
    [Route("api/bookReturn")]
    public class BookReturnController : ControllerBase
    {
        private readonly IBookReturnService _bookReturnService;

        public BookReturnController(IBookReturnService bookReturnService)
        {
            _bookReturnService = bookReturnService;
        }

        [HttpGet("GetAllBookReturn")]
        public async Task<IActionResult> GetAllBookReturn()
        {
            Response response = new Response();
            try
            {
                var result = await _bookReturnService.GetAllBookReturnService();
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

        [HttpGet("GetBookReturnbyId")]
        public async Task<IActionResult> GetBookReturnbyId([FromQuery] int bookReturnId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (bookReturnId <= 0)
                {
                    return BadRequest("Invalid Book Return Id.");
                }

                var result = await _bookReturnService.GetBookReturnByIdService(bookReturnId);

                if (result == null)
                {
                    return NotFound("Book Return not found.");
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
        [HttpPost("CreateBookReturn")]
        public async Task<IActionResult> CreateBookReturn(BookReturnRequest request)
        {
            Response response = new Response();
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
            if (request.StatusId.HasValue && request.StatusId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "StatusId is Required!";
                return Ok(response);
            }
            if (request.ReturnedDate.HasValue && request.ReturnedDate == default(DateTime))
            {
                response.statuscode = Constants.FailCode;
                response.message = "ReturnedDate is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookReturnService.CreateBookReturnService(request);
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
        [HttpPut("UpdateBookReturn")]
        public async Task<IActionResult> UpdateBookReturn([FromBody] BookReturnRequest request)
        {
            Response response = new Response();
            try
            {
                if (request.BookReturnId.HasValue && request.BookReturnId == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = "BookReturnId is Required!";
                    return Ok(response);
                }

                var result = await _bookReturnService.UpdateBookReturnService(request);
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
        [HttpDelete("DeleteBookReturn")]
        public async Task<IActionResult> DeleteBookReturn(int bookReturnId)
        {
            Response response = new Response();
            if (bookReturnId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookReturnId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookReturnService.DeleteBookReturnService(bookReturnId);
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
