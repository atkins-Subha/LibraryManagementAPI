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
    public interface IBookBorrowService
    {

        Task<IEnumerable<BookBorrow>> GetAllBookBorrowService();
        Task<BookBorrow> GetBookBorrowByIdService(int bookId);
        Task<BookBorrow> CreateBookBorrowService(BookBorrow bookBorrow);

        Task DeleteBookBorrowService(BookBorrow bookBorrow);

        Task<Response> CreateBookBorrowService(BookBorrowRequest request);

        Task<Response> UpdateBookBorrowService(BookBorrowRequest request);

        
        Task<Response> DeleteBookBorrowService(int bookBorrowId);

    }
    public class BookBorrowService : IBookBorrowService
    {
        private readonly IBookBorrowRepository _bookBorrowRepository;
        private readonly IConfiguration _configuration;

        public BookBorrowService(IBookBorrowRepository bookBorrowRepository, IConfiguration configuration)
        {
            _bookBorrowRepository = bookBorrowRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<BookBorrow>> GetAllBookBorrowService()
        {
            return await _bookBorrowRepository.GetAllBookBorrowRepo();
        }

        public async Task<BookBorrow> GetBookBorrowByIdService(int bookId)
        {
            return await _bookBorrowRepository.GetBookBorrowByIdRepo(bookId);
        }


        public async Task<BookBorrow> CreateBookBorrowService(BookBorrow book)
        {
            return await _bookBorrowRepository.CreateBookBorrowRepo(book);
        }

        //public async Task UpdateBookBorrowService(BookBorrow book)
        //{
        //    await _bookBorrowRepository.UpdateBookBorrowRepo(book);
        //}
        public async Task DeleteBookBorrowService(BookBorrow book)
        {
            await _bookBorrowRepository.DeleteBookBorrowRepo(book);
        }


        public async Task<Response> CreateBookBorrowService(BookBorrowRequest request)
        {
            Response response = new Response();
            try
            {
               
                var dbBookBorrow = _bookBorrowRepository.GetAllBookBorrowRepo().Result.FirstOrDefault(p => p.UserId == request.UserId && p.BookId == request.BookId && p.StatusId == request.StatusId);

                if ((dbBookBorrow == null))
                {

                    var newBookBorrow = new BookBorrow
                    {
                        UserId = request.UserId,
                        BookId = request.BookId,
                        StatusId = request.StatusId,
                        BorrowedDate = request.BorrowedDate,
                        DueDate = request.DueDate,  
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    BookBorrow newBookDetatils = await _bookBorrowRepository.CreateBookBorrowRepo(newBookBorrow);
                    response.statuscode = Constants.SuccessCode;
                    response.data = newBookDetatils;
                    //response.data = null;
                    response.message = Constants.bookBorrowCreatedMsg;
                    return response;

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookBorrowNotCreatedMsg;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }
            return response;
        }


        public async Task<Response> UpdateBookBorrowService(BookBorrowRequest request)
        {

            Response response = new Response();
            var dbBookDetail = _bookBorrowRepository.GetBookBorrowByIdRepo(request.BookBorrowId.Value).Result;


            if ((dbBookDetail != null))
            {
                try
                {
                    if (request.BookBorrowId.HasValue && request.BookBorrowId > 0)
                    {
                        dbBookDetail.BookBorrowId = request.BookBorrowId.Value;
                    }
                    if (request.UserId.HasValue && request.UserId > 0)
                    {
                        dbBookDetail.UserId = request.UserId;
                    }
                    if (request.BookId.HasValue && request.BookId > 0)
                    {
                        dbBookDetail.BookId = request.BookId;
                    }
                    if (request.BorrowedDate.HasValue && request.BorrowedDate != default(DateTime))
                    {
                        dbBookDetail.BorrowedDate = request.BorrowedDate;
                    }
                    if (request.DueDate.HasValue && request.DueDate != default(DateTime))
                    {
                        dbBookDetail.DueDate = request.DueDate;
                    }
                    if (request.StatusId.HasValue && request.StatusId > 0)
                    {
                        dbBookDetail.StatusId = request.StatusId;
                        dbBookDetail.Status = null;
                    }

                    dbBookDetail.UpdatedAt = DateTime.UtcNow;

                    await _bookBorrowRepository.UpdateBookBorrowRepo(dbBookDetail);

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

        public async Task<Response> DeleteBookBorrowService(int bookBorrowId)
        {
            Response response = new Response();
            try
            {
                BookBorrow bookBorrow = await _bookBorrowRepository.GetBookBorrowByIdRepo(bookBorrowId);
                if (bookBorrow != null)
                {
                    await _bookBorrowRepository.DeleteBookBorrowRepo(bookBorrow);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.bookBorrowDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookBorrowNotDeletedMsg;
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
