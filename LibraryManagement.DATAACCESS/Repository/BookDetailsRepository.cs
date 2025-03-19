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
    public interface IBookDetailsRepository : IRepositoryBase<BookDetails>
    {
        Task<IEnumerable<BookDetails>> GetAllBookDetailsRepo();
        Task<BookDetails> GetBookDetailsByIdRepo(int bookDetailsId);
        Task<BookDetails> CreateBookDetailsRepo(BookDetails bookDetails);
        Task UpdateBookDetailsRepo(BookDetails bookDetails);
        Task DeleteBookDetailsRepo(BookDetails bookDetails);
    }
    public class BookDetailsRepository : RepositoryBase<BookDetails>, IBookDetailsRepository
    {
        public BookDetailsRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<BookDetails>> GetAllBookDetailsRepo()
        {

            return await FindAllBaseRepo()
                .Include(e => e.User)
                .Include(e => e.Book)
                .Include(e => e.Status)
                .Include(e => e.Book.Category)
            .ToListAsync();

        }

        public async Task<BookDetails> GetBookDetailsByIdRepo(int bookDetailsId)
        {
            return await FindByConditionBaseRepo(e => e.BookDetailsId.Equals(bookDetailsId))
                .Include(e => e.User)
                .Include(e => e.Book)
                .Include(e => e.Status)
                .Include(e => e.Book.Category)
            .FirstOrDefaultAsync();
        }

        public async Task<BookDetails> CreateBookDetailsRepo(BookDetails bookDetails)
        {
            return await CreateBaseRepo(bookDetails);
        }
        public async Task UpdateBookDetailsRepo(BookDetails bookDetails)
        {
            await UpdateBaseRepo(bookDetails);
        }

        public async Task DeleteBookDetailsRepo(BookDetails bookDetails)
        {
            await DeleteBaseRepo(bookDetails);
        }
    }
}
