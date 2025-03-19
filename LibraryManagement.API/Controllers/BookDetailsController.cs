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
    [Route("api/bookdetails")]
    public class BookDetailsController : ControllerBase
    {
        private readonly IBookDetailsService _bookDetailsService;

        public BookDetailsController(IBookDetailsService bookDetailsService)
        {
            _bookDetailsService = bookDetailsService;
        }

        [HttpGet("GetAllBookDetails")]
        public async Task<IActionResult> GetAllBookDetails()
        {
            Response response = new Response();
            try
            {
                var result = await _bookDetailsService.GetAllBookDetailsService();
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


        [HttpPost("GetAllBookDetailsbyStatus")]
        public async Task<IActionResult> GetAllBookDetailsbyStatus([FromBody] BorrowedBooksRequest request)
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
                var lstAllBookDetails = await _bookDetailsService.GetAllBookDetailsService();

                //LINQ query to filter books based on Author or Category
                var result = lstAllBookDetails
                    .Where(b =>  b.UserId == request.UserId)
                    .ToList();


                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    result = result
                   .Where(b => b.Book.Author.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase)
                            || b.Book.Category.CategoryName.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase))
                   .ToList();                  
                }

                if (request.StartDate.HasValue && request.EndDate.HasValue)
                {
                   // result = result
                   //.Where(b => b.BorrowedDate >= request.StartDate.Value && b.BorrowedDate <= request.EndDate.Value)
                   //.ToList();
                }

                if (result.Count == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.data = result;
                    response.message = Constants.NoDataFoundMsg;
                }

                response.statuscode = Constants.SuccessCode;
                response.data = result;
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


        [HttpGet("GetBookDetailsbyId")]
        public async Task<IActionResult> GetBookDetailsbyId([FromQuery] int bookDetailId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (bookDetailId <= 0)
                {
                    return BadRequest("Invalid Book Datail Id.");
                }

                var result = await _bookDetailsService.GetBookDetailsByIdService(bookDetailId);

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
        [HttpPost("CreateBookDetails")]
        public async Task<IActionResult> CreateBookDetails(BookDetailsRequest request)
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
            if (request.DueDate.HasValue && request.DueDate == default(DateTime))
            {
                response.statuscode = Constants.FailCode;
                response.message = "DueDate is Required!";
                return Ok(response);
            }

            try
            {
                var result = await _bookDetailsService.CreateBookDetailsService(request);
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
        [HttpPut("UpdateBookDetails")]
        public async Task<IActionResult> UpdateBookDetails([FromBody] BookDetailsRequest request)
        {
            Response response = new Response();
            try
            {
                if (request.BookDetailsId.HasValue && request.BookDetailsId == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = "BookDetailsId is Required!";
                    return Ok(response);
                }

                var result = await _bookDetailsService.UpdateBookDetailsService(request);
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

        //[HttpPut("UpdateBookDetails")]
        //public async Task<IActionResult> UpdateBookDetails([FromBody] BookDetailsStatusRequest request)
        //{
        //    Response response = new Response();
        //    try
        //    {
        //        var result = await _bookDetailsService.UpdateBookDetailsService(request);
        //        //var result = "";
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
        //        response.statuscode = Constants.ErrorCode;
        //        response.message = ex.Message;
        //        return Ok(response);
        //    }
        //}

        //[Authorize(Roles = "Librarian")]
        [HttpDelete("DeleteBookDetails")]
        public async Task<IActionResult> DeleteBookDetails(int bookDetailsId)
        {
            Response response = new Response();
            if (bookDetailsId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookDetailsId is Required!";
                return Ok(response);
            }
            try
            {

                //Todo Delete bookdetails first


                var result = await _bookDetailsService.DeleteBookDetailsService(bookDetailsId);
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
