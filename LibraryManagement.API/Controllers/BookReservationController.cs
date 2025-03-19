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
    [Route("api/bookReservation")]
    public class BookReservationController : ControllerBase
    {
        private readonly IBookReservationService _bookReservationService;

        public BookReservationController(IBookReservationService bookReservationService)
        {
            _bookReservationService = bookReservationService;
        }

        [HttpGet("GetAllBookReservation")]
        public async Task<IActionResult> GetAllBookReservation()
        {
            Response response = new Response();
            try
            {
                var result = await _bookReservationService.GetAllBookReservationService();
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

        [HttpGet("GetBookReservationbyId")]
        public async Task<IActionResult> GetBookReservationbyId([FromQuery] int bookReservationId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (bookReservationId <= 0)
                {
                    return BadRequest("Invalid Book Reservation Id.");
                }

                var result = await _bookReservationService.GetBookReservationByIdService(bookReservationId);

                if (result == null)
                {
                    return NotFound("Book Reservation not found.");
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
        [HttpPost("CreateBookReservation")]
        public async Task<IActionResult> CreateBookReservation(BookReservationRequest request)
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
            if (request.ReservedDate.HasValue && request.ReservedDate == default(DateTime))
            {
                response.statuscode = Constants.FailCode;
                response.message = "ReservedDate is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookReservationService.CreateBookReservationService(request);
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
        [HttpPut("UpdateBookReservation")]
        public async Task<IActionResult> UpdateBookReservation([FromBody] BookReservationRequest request)
        {
            Response response = new Response();
            try
            {
                if (request.BookReservationId.HasValue && request.BookReservationId == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = "BookReservationId is Required!";
                    return Ok(response);
                }

                var result = await _bookReservationService.UpdateBookReservationService(request);
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
        [HttpDelete("DeleteBookReservation")]
        public async Task<IActionResult> DeleteBookReservation(int bookReservationId)
        {
            Response response = new Response();
            if (bookReservationId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookReservationId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookReservationService.DeleteBookReservationService(bookReservationId);
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
