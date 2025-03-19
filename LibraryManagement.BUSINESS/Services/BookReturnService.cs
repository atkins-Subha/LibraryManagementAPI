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
    public interface IBookReturnService
    {

        Task<IEnumerable<BookReturn>> GetAllBookReturnService();
        Task<BookReturn> GetBookReturnByIdService(int bookId);
        Task<BookReturn> CreateBookReturnService(BookReturn bookReturn);

        Task DeleteBookReturnService(BookReturn bookReturn);

        Task<Response> CreateBookReturnService(BookReturnRequest request);

        Task<Response> UpdateBookReturnService(BookReturnRequest request);

        
        Task<Response> DeleteBookReturnService(int bookReturnId);

    }
    public class BookReturnService : IBookReturnService
    {
        private readonly IBookReturnRepository _bookReturnRepository;
        private readonly IConfiguration _configuration;

        public BookReturnService(IBookReturnRepository bookReturnRepository, IConfiguration configuration)
        {
            _bookReturnRepository = bookReturnRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<BookReturn>> GetAllBookReturnService()
        {
            return await _bookReturnRepository.GetAllBookReturnRepo();
        }

        public async Task<BookReturn> GetBookReturnByIdService(int bookId)
        {
            return await _bookReturnRepository.GetBookReturnByIdRepo(bookId);
        }


        public async Task<BookReturn> CreateBookReturnService(BookReturn book)
        {
            return await _bookReturnRepository.CreateBookReturnRepo(book);
        }

        //public async Task UpdateBookReturnService(BookReturn book)
        //{
        //    await _bookReturnRepository.UpdateBookReturnRepo(book);
        //}
        public async Task DeleteBookReturnService(BookReturn book)
        {
            await _bookReturnRepository.DeleteBookReturnRepo(book);
        }


        public async Task<Response> CreateBookReturnService(BookReturnRequest request)
        {
            Response response = new Response();
            try
            {
               
                var dbBookReturn = _bookReturnRepository.GetAllBookReturnRepo().Result.FirstOrDefault(p => p.UserId == request.UserId && p.BookId == request.BookId && p.StatusId == request.StatusId);

                if ((dbBookReturn == null))
                {

                    var newBookReturn = new BookReturn
                    {
                        UserId = request.UserId,
                        BookId = request.BookId,
                        StatusId = request.StatusId,
                        ReturnedDate = request.ReturnedDate,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    BookReturn newBookDetatils = await _bookReturnRepository.CreateBookReturnRepo(newBookReturn);
                    response.statuscode = Constants.SuccessCode;
                    response.data = newBookDetatils;
                    //response.data = null;
                    response.message = Constants.bookReturnCreatedMsg;
                    return response;

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookReturnNotCreatedMsg;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }
            return response;
        }


        public async Task<Response> UpdateBookReturnService(BookReturnRequest request)
        {

            Response response = new Response();
            var dbBookDetail = _bookReturnRepository.GetBookReturnByIdRepo(request.BookReturnId.Value).Result;


            if ((dbBookDetail != null))
            {
                try
                {
                    if (request.BookReturnId.HasValue && request.BookReturnId > 0)
                    {
                        dbBookDetail.BookReturnId = request.BookReturnId.Value;
                    }
                    if (request.UserId.HasValue && request.UserId > 0)
                    {
                        dbBookDetail.UserId = request.UserId;
                    }
                    if (request.BookId.HasValue && request.BookId > 0)
                    {
                        dbBookDetail.BookId = request.BookId;
                    }
                    if (request.ReturnedDate.HasValue && request.ReturnedDate != default(DateTime))
                    {
                        dbBookDetail.ReturnedDate = request.ReturnedDate;
                    }
                    if (request.StatusId.HasValue && request.StatusId > 0)
                    {
                        dbBookDetail.StatusId = request.StatusId;
                        dbBookDetail.Status = null;
                    }

                    dbBookDetail.UpdatedAt = DateTime.UtcNow;

                    await _bookReturnRepository.UpdateBookReturnRepo(dbBookDetail);

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

        public async Task<Response> DeleteBookReturnService(int bookReturnId)
        {
            Response response = new Response();
            try
            {
                BookReturn bookReturn = await _bookReturnRepository.GetBookReturnByIdRepo(bookReturnId);
                if (bookReturn != null)
                {
                    await _bookReturnRepository.DeleteBookReturnRepo(bookReturn);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.bookReturnDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookReturnNotDeletedMsg;
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
