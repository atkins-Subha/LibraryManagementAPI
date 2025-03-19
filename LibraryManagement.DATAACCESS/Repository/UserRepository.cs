using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagement.DATAACCESS.Repository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUsersRepo();
        Task<User> GetUserByIdRepo(int userId);
        Task<User> CreateUserRepo(User user);
        Task UpdateUserRepo(User user);
        Task DeleteUserRepo(User user);
    }
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersRepo()
        {
            return await FindAllBaseRepo()
             .Include(e => e.Role)
            .OrderByDescending(e => e.UserId)
            .ToListAsync();
        }

        public async Task<User> GetUserByIdRepo(int userId)
        {
            return await FindByConditionBaseRepo(e => e.UserId.Equals(userId)).Include(e => e.Role)
            .FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserRepo(User user)
        {
            return await CreateBaseRepo(user);
        }
        public async Task UpdateUserRepo(User user)
        {
            await UpdateBaseRepo(user);
        }

        public async Task DeleteUserRepo(User user)
        {
            await DeleteBaseRepo(user);
        }
    }
}
