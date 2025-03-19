using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.DATAACCESS.Repository;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUSINESS.Services
{
    public interface IBookService
    {

        Task<IEnumerable<Book>> GetAllBooksService();
        Task<Book> GetBookByIdService(int bookId);
        Task<Book> CreateBookService(Book book);
        Task UpdateBookService(Book book);
        Task DeleteBookService(Book book);

        Task<Response> CreateBookService(BookRequest request);

        Task<Response> UpdateBookService(BookRequest request);
        //Task<Response> UpdateAvailableBookCountService(int bookId, int noOfAvailableBook);
        Task<Response> DeleteBookService(int bookId);

    }
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IConfiguration _configuration;

        public BookService(IBookRepository bookRepository, IConfiguration configuration)
        {
            _bookRepository = bookRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Book>> GetAllBooksService()
        {
            return await _bookRepository.GetAllBooksRepo();
        }

        public async Task<Book> GetBookByIdService(int bookId)
        {
            return await _bookRepository.GetBookByIdRepo(bookId);
        }


        public async Task<Book> CreateBookService(Book book)
        {
            return await _bookRepository.CreateBookRepo(book);
        }

        public async Task UpdateBookService(Book book)
        {
            await _bookRepository.UpdateBookRepo(book);
        }
        public async Task DeleteBookService(Book book)
        {
            await _bookRepository.DeleteBookRepo(book);
        }

        public async Task<Response> CreateBookService(BookRequest request)
        {
            Response response = new Response();
            try
            {
                var dbBook = _bookRepository.GetAllBooksRepo().Result.FirstOrDefault(p => p.ISBN.Trim() == request.ISBN.Trim());

                if ((dbBook == null))
                {

                    var book = new Book
                    {
                        Title = request.Title,
                        Author = request.Author,
                        Genre = request.Genre,
                        Publisher = request.Publisher,
                        ISBN = request.ISBN,
                        NoOfBooks = request.NoOfBooks.Value,
                        CategoryId = request.CategoryId.Value,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };


                    Book newBook = await _bookRepository.CreateBookRepo(book);
                    response.statuscode = Constants.SuccessCode;
                    response.data = newBook;
                    response.message = Constants.bookCreatedMsg;
                    return response;
                }

                response.statuscode = Constants.FailCode;
                response.message = Constants.bookNotCreatedMsg;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }


            return response;


        }

        public async Task<Response> UpdateBookService(BookRequest request)
        {

            Response response = new Response();
            var dbBook = _bookRepository.GetBookByIdRepo(request.BookId.Value).Result;


            if ((dbBook != null))
            {
                try
                {
                    if (request.BookId.HasValue && request.BookId > 0)
                    {
                        dbBook.BookId = request.BookId.Value;
                    }
                    if (!string.IsNullOrEmpty(request.Title))
                    {
                        dbBook.Title = request.Title;
                    }
                    if (!string.IsNullOrEmpty(request.Author))
                    {
                        dbBook.Author = request.Author;
                    }
                    if (!string.IsNullOrEmpty(request.Genre))
                    {
                        dbBook.Genre = request.Genre;
                    }
                    if (!string.IsNullOrEmpty(request.Publisher))
                    {
                        dbBook.Publisher = request.Publisher;
                    }
                    if (!string.IsNullOrEmpty(request.ISBN))
                    {
                        dbBook.ISBN = request.ISBN;
                    }
                    if (request.NoOfBooks.HasValue && request.NoOfBooks > 0)
                    {
                        dbBook.NoOfBooks = request.NoOfBooks.Value;
                    }
                    if (request.CategoryId.HasValue && request.CategoryId > 0)
                    {
                        dbBook.CategoryId = request.CategoryId.Value;
                        dbBook.Category = null;
                    }

                    dbBook.CreatedAt = DateTime.UtcNow;
                    dbBook.UpdatedAt = DateTime.UtcNow;

                    await _bookRepository.UpdateBookRepo(dbBook);

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


        public async Task<Response> DeleteBookService(int bookId)
        {
            Response response = new Response();
            try
            {
                Book book = await _bookRepository.GetBookByIdRepo(bookId);
                if(book != null)
                {
                    await _bookRepository.DeleteBookRepo(book);

                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.bookDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.bookNotDeletedMsg;
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
