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
    public interface IBookDetailsService
    {

        Task<IEnumerable<BookDetails>> GetAllBookDetailsService();
        Task<BookDetails> GetBookDetailsByIdService(int bookId);
        Task<BookDetails> CreateBookDetailsService(BookDetails bookDetails);

        Task DeleteBookDetailsService(BookDetails bookDetails);

        Task<Response> CreateBookDetailsService(BookDetailsRequest request);

        Task<Response> UpdateBookDetailsService(BookDetailsRequest request);

        
        Task<Response> DeleteBookDetailsService(int bookDetailsId);

    }
    public class BookDetailsService : IBookDetailsService
    {
        private readonly IBookDetailsRepository _bookDetailsRepository;
        private readonly IConfiguration _configuration;

        public BookDetailsService(IBookDetailsRepository bookDetailsRepository, IConfiguration configuration)
        {
            _bookDetailsRepository = bookDetailsRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<BookDetails>> GetAllBookDetailsService()
        {
            return await _bookDetailsRepository.GetAllBookDetailsRepo();
        }

        public async Task<BookDetails> GetBookDetailsByIdService(int bookId)
        {
            return await _bookDetailsRepository.GetBookDetailsByIdRepo(bookId);
        }


        public async Task<BookDetails> CreateBookDetailsService(BookDetails book)
        {
            return await _bookDetailsRepository.CreateBookDetailsRepo(book);
        }

        //public async Task UpdateBookDetailsService(BookDetails book)
        //{
        //    await _bookDetailsRepository.UpdateBookDetailsRepo(book);
        //}
        public async Task DeleteBookDetailsService(BookDetails book)
        {
            await _bookDetailsRepository.DeleteBookDetailsRepo(book);
        }


        public async Task<Response> CreateBookDetailsService(BookDetailsRequest request)
        {
            Response response = new Response();
            try
            {
               
                var dbBookDetails = _bookDetailsRepository.GetAllBookDetailsRepo().Result.FirstOrDefault(p => p.UserId == request.UserId && p.BookId == request.BookId && p.StatusId == request.StatusId);

                if ((dbBookDetails == null))
                {

                    var newBookDetails = new BookDetails
                    {
                        UserId = request.UserId,
                        BookId = request.BookId,
                        DueDate = request.DueDate,
                        StatusId = request.StatusId,

                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    BookDetails newBookDetatils = await _bookDetailsRepository.CreateBookDetailsRepo(newBookDetails);
                    response.statuscode = Constants.SuccessCode;
                    response.data = newBookDetatils;
                    //response.data = null;
                    response.message = Constants.booksDetailsCreatedMsg;
                    return response;

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.booksDetailsNotCreatedMsg;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }
            return response;
        }


        public async Task<Response> UpdateBookDetailsService(BookDetailsRequest request)
        {

            Response response = new Response();
            var dbBookDetail = _bookDetailsRepository.GetBookDetailsByIdRepo(request.BookDetailsId.Value).Result;


            if ((dbBookDetail != null))
            {
                try
                {
                    if (request.BookDetailsId.HasValue && request.BookDetailsId > 0)
                    {
                        dbBookDetail.BookDetailsId = request.BookDetailsId.Value;
                    }
                    if (request.UserId.HasValue && request.UserId > 0)
                    {
                        dbBookDetail.UserId = request.UserId;
                    }
                    if (request.BookId.HasValue && request.BookId > 0)
                    {
                        dbBookDetail.BookId = request.BookId;
                    }
                    if (request.StatusId.HasValue && request.StatusId > 0)
                    {
                        dbBookDetail.StatusId = request.StatusId;
                        dbBookDetail.Status = null;
                    }
                    if (request.DueDate.HasValue && request.DueDate != default(DateTime))
                    {
                        dbBookDetail.DueDate = request.DueDate;
                    }
                    

                    dbBookDetail.UpdatedAt = DateTime.UtcNow;

                    await _bookDetailsRepository.UpdateBookDetailsRepo(dbBookDetail);

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

        public async Task<Response> DeleteBookDetailsService(int bookDetailsId)
        {
            Response response = new Response();
            try
            {
                BookDetails bookDetails = await _bookDetailsRepository.GetBookDetailsByIdRepo(bookDetailsId);
                if (bookDetails != null)
                {
                    await _bookDetailsRepository.DeleteBookDetailsRepo(bookDetails);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.booksDetailsDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.booksDetailsNotDeletedMsg;
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
