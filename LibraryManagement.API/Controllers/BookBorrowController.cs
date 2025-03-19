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
    [Route("api/bookborrow")]
    public class BookBorrowController : ControllerBase
    {
        private readonly IBookBorrowService _bookBorrowService;

        public BookBorrowController(IBookBorrowService bookBorrowService)
        {
            _bookBorrowService = bookBorrowService;
        }

        [HttpGet("GetAllBookBorrow")]
        public async Task<IActionResult> GetAllBookBorrow()
        {
            Response response = new Response();
            try
            {
                var result = await _bookBorrowService.GetAllBookBorrowService();
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

        [HttpGet("GetBookBorrowCountbyUser")]
        public async Task<IActionResult> GetBookBorrowCountbyUser([FromQuery] int userId)
        {
            Response response = new Response();
            try
            {
                var result = _bookBorrowService.GetAllBookBorrowService().Result.Where(x => x.UserId == userId);
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


        [HttpPost("GetAllBookBorrowedbyUser")]
        public async Task<IActionResult> GetAllBookBorrowedbyUser([FromBody] BorrowedBooksRequest request)
        {
            Response response = new Response();

            if (request.UserId.HasValue && request.UserId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserId is Required!";
                return Ok(response);
            }
            if (request.StartDate.HasValue && request.StartDate == default(DateTime))
            {
                response.statuscode = Constants.FailCode;
                response.message = "BorrowedDate is Required!";
                return Ok(response);
            }
            if (request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate.Value >= request.EndDate.Value)
            {
                response.statuscode = Constants.FailCode;
                response.message = "End date must be greater than start date";
                return Ok(response);
            }
            try
            {
                var lstBookBorrow = _bookBorrowService.GetAllBookBorrowService().Result.Where(x => x.UserId == request.UserId).ToList();

                //LINQ query to filter books based on Author or Category
                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    lstBookBorrow = lstBookBorrow
                   .Where(b => b.Book.Author.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase)
                            || b.Book.Category.CategoryName.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase))
                   .ToList();
                }

                if (request.StartDate.HasValue && request.EndDate.HasValue)
                {
                    lstBookBorrow = lstBookBorrow
                   .Where(b => b.BorrowedDate >= request.StartDate.Value && b.BorrowedDate <= request.EndDate.Value)
                   .ToList();
                }

                if (lstBookBorrow.Count == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.data = lstBookBorrow;
                    response.message = Constants.NoDataFoundMsg;
                }

                response.statuscode = Constants.SuccessCode;
                response.data = lstBookBorrow;
                return Ok(response);

            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
        }


        [HttpGet("GetBookBorrowbyId")]
        public async Task<IActionResult> GetBookBorrowbyId([FromQuery] int bookBorrowId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (bookBorrowId <= 0)
                {
                    return BadRequest("Invalid Book borrow Id.");
                }

                var result = await _bookBorrowService.GetBookBorrowByIdService(bookBorrowId);

                if (result == null)
                {
                    return NotFound("Book borrow not found.");
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
        [HttpPost("CreateBookBorrow")]
        public async Task<IActionResult> CreateBookBorrow(BookBorrowRequest request)
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
            if (request.BorrowedDate.HasValue && request.BorrowedDate == default(DateTime))
            {
                response.statuscode = Constants.FailCode;
                response.message = "BorrowedDate is Required!";
                return Ok(response);
            }
            if (request.DueDate.HasValue && request.DueDate == default(DateTime))
            {
                response.statuscode = Constants.FailCode;
                response.message = "DueDate is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookBorrowService.CreateBookBorrowService(request);
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
        [HttpPut("UpdateBookBorrow")]
        public async Task<IActionResult> UpdateBookBorrow([FromBody] BookBorrowRequest request)
        {
            Response response = new Response();
            try
            {
                if (request.BookBorrowId.HasValue && request.BookBorrowId == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = "BookBorrowId is Required!";
                    return Ok(response);
                }

                var result = await _bookBorrowService.UpdateBookBorrowService(request);
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
        [HttpDelete("DeleteBookBorrow")]
        public async Task<IActionResult> DeleteBookBorrow(int bookBorrowId)
        {
            Response response = new Response();
            if (bookBorrowId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookBorrowId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookBorrowService.DeleteBookBorrowService(bookBorrowId);
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
