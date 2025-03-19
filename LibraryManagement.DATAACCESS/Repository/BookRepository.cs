using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagement.DATAACCESS.Repository
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetAllBooksRepo();
        Task<Book> GetBookByIdRepo(int bookId);
        Task<Book> CreateBookRepo(Book book);
        Task UpdateBookRepo(Book book);
        Task DeleteBookRepo(Book book);
    }
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<Book>> GetAllBooksRepo()
        {
            return await FindAllBaseRepo()
             .Include(e => e.Category)
            .OrderByDescending(e => e.BookId)
            .ToListAsync();
        }

        public async Task<Book> GetBookByIdRepo(int bookId)
        {
            return await FindByConditionBaseRepo(e => e.BookId.Equals(bookId)).Include(e => e.Category)
            .FirstOrDefaultAsync();
        }

        public async Task<Book> CreateBookRepo(Book book)
        {
            return await CreateBaseRepo(book);
        }
        public async Task UpdateBookRepo(Book book)
        {
            await UpdateBaseRepo(book);
        }

        public async Task DeleteBookRepo(Book book)
        {
            await DeleteBaseRepo(book);
        }
    }
}
