using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DATAACCESS.Repository
{
    public interface IBookAlertsRepository : IRepositoryBase<BookAlerts>
    {
        Task<IEnumerable<BookAlerts>> GetAllBookAlertsRepo();
        Task<BookAlerts> GetBookAlertsByIdRepo(int bookAlertsId);
        Task<BookAlerts> CreateBookAlertsRepo(BookAlerts bookAlerts);
        Task UpdateBookAlertsRepo(BookAlerts bookAlerts);
        Task DeleteBookAlertsRepo(BookAlerts bookAlerts);
    }
    public class BookAlertsRepository : RepositoryBase<BookAlerts>, IBookAlertsRepository
    {
        public BookAlertsRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<BookAlerts>> GetAllBookAlertsRepo()
        {

            return await FindAllBaseRepo()
                .Include(e => e.User)
                .Include(e => e.Book)
            .ToListAsync();

        }

        public async Task<BookAlerts> GetBookAlertsByIdRepo(int bookAlertsId)
        {
            return await FindByConditionBaseRepo(e => e.BookAlertsId.Equals(bookAlertsId))
                .Include(e => e.User)
                .Include(e => e.Book)
            .FirstOrDefaultAsync();
        }

        public async Task<BookAlerts> CreateBookAlertsRepo(BookAlerts bookAlerts)
        {
            return await CreateBaseRepo(bookAlerts);
        }
        public async Task UpdateBookAlertsRepo(BookAlerts bookAlerts)
        {
            await UpdateBaseRepo(bookAlerts);
        }

        public async Task DeleteBookAlertsRepo(BookAlerts bookAlerts)
        {
            await DeleteBaseRepo(bookAlerts);
        }
    }
}
