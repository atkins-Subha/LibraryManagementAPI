using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DATAACCESS.Repository
{
    public interface IBookBorrowRepository : IRepositoryBase<BookBorrow>
    {
        Task<IEnumerable<BookBorrow>> GetAllBookBorrowRepo();
        Task<BookBorrow> GetBookBorrowByIdRepo(int bookBorrowId);
        Task<BookBorrow> CreateBookBorrowRepo(BookBorrow bookBorrow);
        Task UpdateBookBorrowRepo(BookBorrow bookBorrow);
        Task DeleteBookBorrowRepo(BookBorrow bookBorrow);
    }
    public class BookBorrowRepository : RepositoryBase<BookBorrow>, IBookBorrowRepository
    {
        public BookBorrowRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<BookBorrow>> GetAllBookBorrowRepo()
        {

            return await FindAllBaseRepo()
                .Include(e => e.User)
                .Include(e => e.Book)
                 .Include(e => e.Book.Category)
                .Include(e => e.Status)
            .ToListAsync();

        }

        public async Task<BookBorrow> GetBookBorrowByIdRepo(int bookBorrowId)
        {
            return await FindByConditionBaseRepo(e => e.BookBorrowId.Equals(bookBorrowId))
                .Include(e => e.User)
                .Include(e => e.Book)
                 .Include(e => e.Book.Category)
                .Include(e => e.Status)
            .FirstOrDefaultAsync();
        }

        public async Task<BookBorrow> CreateBookBorrowRepo(BookBorrow bookBorrow)
        {
            return await CreateBaseRepo(bookBorrow);
        }
        public async Task UpdateBookBorrowRepo(BookBorrow bookBorrow)
        {
            await UpdateBaseRepo(bookBorrow);
        }

        public async Task DeleteBookBorrowRepo(BookBorrow bookBorrow)
        {
            await DeleteBaseRepo(bookBorrow);
        }
    }
}
