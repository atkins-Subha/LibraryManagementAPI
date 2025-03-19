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
    public interface IBookReturnRepository : IRepositoryBase<BookReturn>
    {
        Task<IEnumerable<BookReturn>> GetAllBookReturnRepo();
        Task<BookReturn> GetBookReturnByIdRepo(int bookReturnId);
        Task<BookReturn> CreateBookReturnRepo(BookReturn bookReturn);
        Task UpdateBookReturnRepo(BookReturn bookReturn);
        Task DeleteBookReturnRepo(BookReturn bookReturn);
    }
    public class BookReturnRepository : RepositoryBase<BookReturn>, IBookReturnRepository
    {
        public BookReturnRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<BookReturn>> GetAllBookReturnRepo()
        {

            return await FindAllBaseRepo()
                .Include(e => e.User)
                .Include(e => e.Book)
                 .Include(e => e.Book.Category)
                .Include(e => e.Status)
            .ToListAsync();

        }

        public async Task<BookReturn> GetBookReturnByIdRepo(int bookReturnId)
        {
            return await FindByConditionBaseRepo(e => e.BookReturnId.Equals(bookReturnId))
                .Include(e => e.User)
                .Include(e => e.Book)
                 .Include(e => e.Book.Category)
                .Include(e => e.Status)
            .FirstOrDefaultAsync();
        }

        public async Task<BookReturn> CreateBookReturnRepo(BookReturn bookReturn)
        {
            return await CreateBaseRepo(bookReturn);
        }
        public async Task UpdateBookReturnRepo(BookReturn bookReturn)
        {
            await UpdateBaseRepo(bookReturn);
        }

        public async Task DeleteBookReturnRepo(BookReturn bookReturn)
        {
            await DeleteBaseRepo(bookReturn);
        }
    }
}
