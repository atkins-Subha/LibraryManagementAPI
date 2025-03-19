using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.DATAACCESS.Repository;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUSINESS.Services
{
    public interface IBookAlertsService
    {

        Task<IEnumerable<BookAlerts>> GetAllBookAlertsService();
        Task<BookAlerts> GetBookAlertsByIdService(int bookId);
        Task<BookAlerts> CreateBookAlertsService(BookAlerts bookAlerts);

        Task DeleteBookAlertsService(BookAlerts bookAlerts);

        Task<Response> CreateBookAlertsService(BookAlertsRequest request);

        Task<Response> UpdateBookAlertsService(BookAlertsRequest request);

        
        Task<Response> DeleteBookAlertsService(int bookAlertsId);

    }
    public class BookAlertsService : IBookAlertsService
    {
        private readonly IBookAlertsRepository _bookAlertsRepository;
        private readonly IConfiguration _configuration;

        public BookAlertsService(IBookAlertsRepository bookAlertsRepository, IConfiguration configuration)
        {
            _bookAlertsRepository = bookAlertsRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<BookAlerts>> GetAllBookAlertsService()
        {
            return await _bookAlertsRepository.GetAllBookAlertsRepo();
        }

        public async Task<BookAlerts> GetBookAlertsByIdService(int bookId)
        {
            return await _bookAlertsRepository.GetBookAlertsByIdRepo(bookId);
        }


        public async Task<BookAlerts> CreateBookAlertsService(BookAlerts book)
        {
            return await _bookAlertsRepository.CreateBookAlertsRepo(book);
        }

        //public async Task UpdateBookAlertsService(BookAlerts book)
        //{
        //    await _bookAlertsRepository.UpdateBookAlertsRepo(book);
        //}
        public async Task DeleteBookAlertsService(BookAlerts book)
        {
            await _bookAlertsRepository.DeleteBookAlertsRepo(book);
        }


        public async Task<Response> CreateBookAlertsService(BookAlertsRequest request)
        {
            Response response = new Response();
            try
            {
               
                var dbBookAlerts = _bookAlertsRepository.GetAllBookAlertsRepo().Result.FirstOrDefault(p => p.BookId == request.BookId && p.UserId == request.UserId);

                if ((dbBookAlerts == null))
                {

                    var bookAlerts = new BookAlerts
                    {
                        BookId = request.BookId,
                        UserId = request.UserId,
                        IsBookBorrowedAlert = request.IsBookBorrowedAlert,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    BookAlerts newBookAlerts = await _bookAlertsRepository.CreateBookAlertsRepo(bookAlerts);
                    response.statuscode = Constants.SuccessCode;
                    response.data = newBookAlerts;
                    //response.data = null;
                    response.message = Constants.bookAlertsCreatedMsg;
                    return response;

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookAlertsNotCreatedMsg;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }
            return response;
        }


        public async Task<Response> UpdateBookAlertsService(BookAlertsRequest request)
        {

            Response response = new Response();
            var dbBookDetail = _bookAlertsRepository.GetBookAlertsByIdRepo(request.BookAlertsId.Value).Result;


            if ((dbBookDetail != null))
            {
                try
                {
                    if (request.BookAlertsId.HasValue && request.BookAlertsId > 0)
                    {
                        dbBookDetail.BookAlertsId = request.BookAlertsId.Value;
                    }
                    if (request.UserId.HasValue && request.UserId > 0)
                    {
                        dbBookDetail.UserId = request.UserId;
                    }
                    if (request.BookId.HasValue && request.BookId > 0)
                    {
                        dbBookDetail.BookId = request.BookId;
                    }
                    if (request.IsBookBorrowedAlert.HasValue)
                    {
                        dbBookDetail.IsBookBorrowedAlert = request.IsBookBorrowedAlert;
                    }
                    if (request.IsBookReturnedAlert.HasValue)
                    {
                        dbBookDetail.IsBookReturnedAlert = request.IsBookReturnedAlert;
                    }

                    dbBookDetail.UpdatedAt = DateTime.UtcNow;

                    await _bookAlertsRepository.UpdateBookAlertsRepo(dbBookDetail);

                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.userUpdatedMsg;
                    return response;

                }
                catch (Exception ex)
                {
                    response.statuscode = -1;
                    response.message = ex.InnerException.Message;
                    return response;
                }

            }
            response.statuscode = 0;
            response.message = Constants.bookNotUpdatedMsg;
            return response;

        }

        public async Task<Response> DeleteBookAlertsService(int bookAlertsId)
        {
            Response response = new Response();
            try
            {
                BookAlerts bookAlerts = await _bookAlertsRepository.GetBookAlertsByIdRepo(bookAlertsId);
                if (bookAlerts != null)
                {
                    await _bookAlertsRepository.DeleteBookAlertsRepo(bookAlerts);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.bookAlertsDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookAlertsNotDeletedMsg;
                return response;

            }
            catch (Exception ex)
            {
                response.statuscode = -1;
                response.message = ex.InnerException.Message;
                return response;
            }

            return response;
        }
    }
}
