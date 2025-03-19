using Azure.Core;
using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.RegularExpressions;

namespace LibraryManagement.API.Controllers
{

    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBookDetailsService _bookDetailsService;
        private readonly ISubscriptionDetailsService _subscriptionDetailsService;
        private readonly IBookAlertsService _bookAlertsService;
        private readonly IBookBorrowService _bookBorrowService;
        private readonly IBookReturnService _bookReturnService;
        private readonly IBookReservationService _bookReservationService;


        public BookController(IBookService bookService, IBookDetailsService bookDetailsService,
               ISubscriptionDetailsService subscriptionDetailsService, ISubscriptionService subscriptionService,
               IBookAlertsService bookAlertsService, IBookBorrowService bookBorrowService, IBookReturnService bookReturnService,
               IBookReservationService bookReservationService)
        {
            _bookService = bookService;
            _bookDetailsService = bookDetailsService;
            _subscriptionDetailsService = subscriptionDetailsService;
            _bookAlertsService = bookAlertsService;
            _bookBorrowService = bookBorrowService;
            _bookReturnService = bookReturnService;
            _bookReservationService = bookReservationService;
        }

        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            Response response = new Response();
            try
            {
                var result = await _bookService.GetAllBooksService();

                response.statuscode = Constants.SuccessCode;
                response.data = result;
                //response.message = Constants.SuccessMsg;


                if (result == null)
                {
                    response.statuscode = Constants.FailCode;
                    response.data = result;
                    response.message = Constants.NoDataFoundMsg;
                }

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

        [HttpPost("GetAllBooksAndBookDetails")]
        public async Task<ActionResult<IEnumerable<BooksAndBookDetailsDTO>>> GetAllBooksAndBookDetails(GetAllBooksAndBookDetailsRequest request)

        {
            Response response = new Response();

            if (request.userId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "User Id is Required!";
                return Ok(response);
            }
            try
            {

                var bookCounts = (from book in _bookService.GetAllBooksService().Result
                                  join bookBorrow in _bookBorrowService.GetAllBookBorrowService().Result.Where(b => b.StatusId == 1)
                                  on book.BookId equals bookBorrow.BookId into borrowGroup
                                  from bgBorrow in borrowGroup.DefaultIfEmpty()
                                  join bookAlert in _bookAlertsService.GetAllBookAlertsService().Result.Where(b => b.IsBookBorrowedAlert == true || b.IsBookReturnedAlert == true)
                                  on book.BookId equals bookAlert.BookId into alertGroup
                                  from bgAlert in alertGroup.DefaultIfEmpty()
                                  group new { bgBorrow, bgAlert } by book.BookId into grouped
                                  select new
                                  {
                                      BookId = grouped.Key,
                                      IsBorrowedCount = grouped.Count(b => b.bgBorrow != null && b.bgBorrow.StatusId == 1),
                                      IsWaitingCount = grouped.Count(b => b.bgBorrow == null &&  b.bgAlert != null),
                                      TotalBorrowCount = grouped.Count(b => b.bgBorrow != null && b.bgBorrow.StatusId == 1) + grouped.Count(b => b.bgBorrow == null && b.bgAlert != null),
                                  }).ToList();


                var lstBooksAndBookDetails = (from lstbook in _bookService.GetAllBooksService().Result

                                              join lstbookBorrow in _bookBorrowService.GetAllBookBorrowService().Result.Where(x => x.UserId == request.userId && x.StatusId == 1)
                                              on lstbook.BookId equals lstbookBorrow.BookId into borrowGroup
                                              from lstbookBorrow in borrowGroup.DefaultIfEmpty()

                                              join lstbookAlerts in _bookAlertsService.GetAllBookAlertsService().Result.Where(x => x.UserId == request.userId)
                                              on lstbook.BookId equals lstbookAlerts?.BookId into AlertsGroup
                                              from lstbookAlerts in AlertsGroup.DefaultIfEmpty()

                                              select new BooksAndBookDetailsDTO
                                              {
                                                  BookId = lstbook.BookId,

                                                  Title = lstbook.Title,

                                                  Author = lstbook.Author,

                                                  CategoryName = lstbook.Category.CategoryName,

                                                  TotalBooksCount = lstbook.NoOfBooks,

                                                  UserBorrowedCount = borrowGroup.Any()
                                                                      ? borrowGroup.Count(bd => bd != null && bd.StatusId == 1)
                                                                      : AlertsGroup.Count(bd => bd != null && bd.IsBookBorrowedAlert == true),

                                                  ConfirmedUserBorrowedCount = borrowGroup.Count(bd => bd != null && bd.StatusId == 1),

                                                  AvailableBooksCount = lstbook.NoOfBooks - 
                                                        bookCounts.FirstOrDefault(r => r.BookId == lstbook.BookId)?.TotalBorrowCount ?? 0


                                              }).ToList();



                if (!string.IsNullOrEmpty(request.searchText))
                {
                    var filterlstBooksAndBookDetails = lstBooksAndBookDetails
                   .Where(b => b.Author.Contains(request.searchText, StringComparison.OrdinalIgnoreCase)
                            || b.CategoryName.Contains(request.searchText, StringComparison.OrdinalIgnoreCase))
                   .ToList();

                    response.statuscode = Constants.SuccessCode;
                    response.data = filterlstBooksAndBookDetails;
                    return Ok(response);
                }

                if (lstBooksAndBookDetails.Count > 0)
                {
                    response.statuscode = Constants.SuccessCode;
                    response.data = lstBooksAndBookDetails;
                    return Ok(response);
                }

                response.statuscode = Constants.FailCode;
                response.message = Constants.NoDataFoundMsg;

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


        [HttpGet("GetBorrowedCountSubscriptionbyUser")]
        public async Task<IActionResult> GetBorrowedCountSubscriptionbyUser([FromQuery] int userId)
        {

            //This is used for check the Eligibilty to borrowed the book
            Response response = new Response();

            if (userId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "User Id is Required!";
                return Ok(response);
            }

            try
            {


                var subscriptionDetails = _subscriptionDetailsService.GetAllSubscriptionDetailsService().Result.Where(x => x.UserId == userId);
                var bookAlerts = _bookAlertsService.GetAllBookAlertsService().Result.Where(x => x.IsBookBorrowedAlert == true);
                var bookBorrows = _bookBorrowService.GetAllBookBorrowService().Result.Where(x => x.StatusId == 1);

                var totalBorrowCount = (from subscriptionDetail in subscriptionDetails
                                        join bookAlert in bookAlerts
                                        on subscriptionDetail.UserId equals bookAlert?.UserId into alertGroup
                                        from ba in alertGroup.DefaultIfEmpty()
                                        join bookBorrow in bookBorrows
                                        on new { UserId = ba?.UserId, BookId = ba?.BookId } equals new { UserId = bookBorrow?.UserId, BookId = bookBorrow?.BookId } into borrowGroup
                                        from bg in borrowGroup.DefaultIfEmpty()
                                        group new { bg, ba, subscriptionDetail } by new { subscriptionDetail.UserId, subscriptionDetail.SubscriptionDetailsId, subscriptionDetail.Subscription } into userGroup
                                        select new BorrowCount
                                        {
                                            UserId = userGroup.Key.UserId,
                                            BooksAllowedCount = userGroup.Key.Subscription.NoOfBooksAllowed,
                                            TotalUserBorrowCount = userGroup.Count(b => b.bg != null) + userGroup.Count(b => b.ba != null && b.bg == null)
                                        }).FirstOrDefault();

                if (totalBorrowCount == null)
                {
                    totalBorrowCount = new BorrowCount
                    {
                        UserId = userId,
                        BooksAllowedCount = 0,
                        TotalUserBorrowCount = 0
                    };
                }

                var result = new
                {
                    UserId = totalBorrowCount.UserId,
                    BooksAllowedCount = totalBorrowCount.BooksAllowedCount,
                    TotalUserBorrowCount = totalBorrowCount.TotalUserBorrowCount
                };
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


        [HttpGet("GetFilteredBooks")]
        public async Task<IActionResult> GetFilteredBooks(string searchText)
        {
            Response response = new Response();
            try
            {
                var lstBooks = _bookService.GetAllBooksService().Result;

                // LINQ query to filter books based on Author or Category
                var result = lstBooks
                    .Where(b => b.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                             || b.Category.CategoryName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                response.statuscode = Constants.SuccessCode;
                response.data = result;
                //response.message = Constants.SuccessMsg;


                if (result.Count == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.data = result;
                    response.message = Constants.NoDataFoundMsg;
                }

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

        [HttpGet("GetBookbyId")]
        public async Task<IActionResult> GetBookbyId([FromQuery] int bookId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (bookId <= 0)
                {
                    return BadRequest("Invalid bookId.");
                }

                var result = await _bookService.GetBookByIdService(bookId);

                if (result == null)
                {
                    return NotFound("Book not found.");
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

        [HttpGet("GetBorrowAlertsbyStatus")]
        public async Task<IActionResult> GetBorrowAlertsbyStatus()
        {
            Response response = new Response();
            try
            {
                //var result = _bookAlertsService.GetAllBookAlertsService().Result.Where(x => x.IsBookBorrowedAlert == request.IsBookBorrowedAlert  && x.IsBookReturnedAlert == request.IsBookReturnedAlert).ToList();

                var result = (from alert in _bookAlertsService.GetAllBookAlertsService().Result
                              join borrow in _bookBorrowService.GetAllBookBorrowService().Result
                              on new { alert.BookId, alert.UserId } equals new { borrow.BookId, borrow.UserId } into borrowGroup
                              from borrow in borrowGroup.DefaultIfEmpty()
                              where alert != null && alert.IsBookBorrowedAlert == true && borrow == null
                              select new
                              {
                                  BookAlertsId = alert.BookAlertsId,
                                  BookId = alert.BookId,
                                  UserId = alert.UserId,
                                  Title = alert.Book.Title,
                                  UserName = alert.User.UserName,
                                  IsBookBorrowedAlert = alert.IsBookBorrowedAlert
                              }).ToList();

                if (result.Count > 0)
                {
                    response.statuscode = Constants.SuccessCode;
                    response.data = result;
                    return Ok(response);
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.NoDataFoundMsg;

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



        //[Authorize(Roles = "Librarian")]
        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook(BookRequest request)
        {
            Response response = new Response();

            if (string.IsNullOrEmpty(request.Title))
            {
                response.statuscode = Constants.FailCode;
                response.message = "Title is Required!";
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.ISBN))
            {
                response.statuscode = Constants.FailCode;
                response.message = "ISBN is Required!";
                return Ok(response);
            }

            if (request.CategoryId.HasValue && request.CategoryId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "CategoryId is Required!";
                return Ok(response);
            }

            try
            {
                var result = await _bookService.CreateBookService(request);
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
        [HttpPut("UpdateBook")]
        public async Task<IActionResult> UpdateBook([FromBody] BookRequest request)
        {
            Response response = new Response();

            if (request.BookId.HasValue && request.BookId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _bookService.UpdateBookService(request);
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

        [HttpDelete("DeleteBook")]
        public async Task<IActionResult> DeleteBook(int bookId, int userId)
        {
            Response response = new Response();
            if (bookId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "BookId is Required!";
                return Ok(response);
            }
            if (userId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserId is Required!";
                return Ok(response);
            }
            try
            {

                //Todo Delete bookdetails first


                var result = await _bookService.DeleteBookService(bookId);
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
