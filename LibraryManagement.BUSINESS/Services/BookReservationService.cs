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
    public interface IBookReservationService
    {

        Task<IEnumerable<BookReservation>> GetAllBookReservationService();
        Task<BookReservation> GetBookReservationByIdService(int bookId);
        Task<BookReservation> CreateBookReservationService(BookReservation bookReservation);

        Task DeleteBookReservationService(BookReservation bookReservation);

        Task<Response> CreateBookReservationService(BookReservationRequest request);

        Task<Response> UpdateBookReservationService(BookReservationRequest request);

        
        Task<Response> DeleteBookReservationService(int bookReservationId);

    }
    public class BookReservationService : IBookReservationService
    {
        private readonly IBookReservationRepository _bookReservationRepository;
        private readonly IConfiguration _configuration;

        public BookReservationService(IBookReservationRepository bookReservationRepository, IConfiguration configuration)
        {
            _bookReservationRepository = bookReservationRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<BookReservation>> GetAllBookReservationService()
        {
            return await _bookReservationRepository.GetAllBookReservationRepo();
        }

        public async Task<BookReservation> GetBookReservationByIdService(int bookId)
        {
            return await _bookReservationRepository.GetBookReservationByIdRepo(bookId);
        }


        public async Task<BookReservation> CreateBookReservationService(BookReservation book)
        {
            return await _bookReservationRepository.CreateBookReservationRepo(book);
        }

        //public async Task UpdateBookReservationService(BookReservation book)
        //{
        //    await _bookReservationRepository.UpdateBookReservationRepo(book);
        //}
        public async Task DeleteBookReservationService(BookReservation book)
        {
            await _bookReservationRepository.DeleteBookReservationRepo(book);
        }


        public async Task<Response> CreateBookReservationService(BookReservationRequest request)
        {
            Response response = new Response();
            try
            {
               
                var dbBookReservation = _bookReservationRepository.GetAllBookReservationRepo().Result.FirstOrDefault(p => p.UserId == request.UserId && p.BookId == request.BookId && p.StatusId == request.StatusId);

                if ((dbBookReservation == null))
                {

                    var newBookReservation = new BookReservation
                    {
                        UserId = request.UserId,
                        BookId = request.BookId,
                        StatusId = request.StatusId,
                        ReservedDate = request.ReservedDate,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    BookReservation newBookDetatils = await _bookReservationRepository.CreateBookReservationRepo(newBookReservation);
                    response.statuscode = Constants.SuccessCode;
                    response.data = newBookDetatils;
                    //response.data = null;
                    response.message = Constants.bookReservationCreatedMsg;
                    return response;

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookReservationNotCreatedMsg;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }
            return response;
        }


        public async Task<Response> UpdateBookReservationService(BookReservationRequest request)
        {

            Response response = new Response();
            var dbBookDetail = _bookReservationRepository.GetBookReservationByIdRepo(request.BookReservationId.Value).Result;


            if ((dbBookDetail != null))
            {
                try
                {
                    if (request.BookReservationId.HasValue && request.BookReservationId > 0)
                    {
                        dbBookDetail.BookReservationId = request.BookReservationId.Value;
                    }
                    if (request.UserId.HasValue && request.UserId > 0)
                    {
                        dbBookDetail.UserId = request.UserId;
                    }
                    if (request.BookId.HasValue && request.BookId > 0)
                    {
                        dbBookDetail.BookId = request.BookId;
                    }
                    if (request.ReservedDate.HasValue && request.ReservedDate != default(DateTime))
                    {
                        dbBookDetail.ReservedDate = request.ReservedDate;
                    }
                    if (request.StatusId.HasValue && request.StatusId > 0)
                    {
                        dbBookDetail.StatusId = request.StatusId;
                        dbBookDetail.Status = null;
                    }
                    if (request.IsUserNotified.HasValue)
                    {
                        dbBookDetail.IsUserNotified = request.IsUserNotified;
                    }

                    dbBookDetail.UpdatedAt = DateTime.UtcNow;

                    await _bookReservationRepository.UpdateBookReservationRepo(dbBookDetail);

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

        public async Task<Response> DeleteBookReservationService(int bookReservationId)
        {
            Response response = new Response();
            try
            {
                BookReservation bookReservation = await _bookReservationRepository.GetBookReservationByIdRepo(bookReservationId);
                if (bookReservation != null)
                {
                    await _bookReservationRepository.DeleteBookReservationRepo(bookReservation);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.bookReservationDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookReservationNotDeletedMsg;
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
