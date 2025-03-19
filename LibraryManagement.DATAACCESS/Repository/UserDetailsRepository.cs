using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagement.DATAACCESS.Repository
{
    public interface IUserDetailsRepository : IRepositoryBase<UserDetails>
    {
        Task<IEnumerable<UserDetails>> GetAllUserDetailsRepo();
        Task<UserDetails> GetUserDetailsByIdRepo(int userDetailsId);
        Task<UserDetails> CreateUserDetailsRepo(UserDetails userDetails);
        Task UpdateUserDetailsRepo(UserDetails user);
        Task DeleteUserDetailsRepo(UserDetails user);
    }
    public class UserDetailsRepository : RepositoryBase<UserDetails>, IUserDetailsRepository
    {
        public UserDetailsRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<UserDetails>> GetAllUserDetailsRepo()
        {
            return await FindAllBaseRepo()
             .Include(e => e.User)
            .OrderByDescending(e => e.UserId)
            .ToListAsync();
        }

        public async Task<UserDetails> GetUserDetailsByIdRepo(int userDetailsId)
        {
            return await FindByConditionBaseRepo(e => e.UserDetailsId.Equals(userDetailsId)).Include(e => e.User)
            .FirstOrDefaultAsync();
        }

        public async Task<UserDetails> CreateUserDetailsRepo(UserDetails userDetails)
        {
            return await CreateBaseRepo(userDetails);
        }
        public async Task UpdateUserDetailsRepo(UserDetails userDetails)
        {
            await UpdateBaseRepo(userDetails);
        }

        public async Task DeleteUserDetailsRepo(UserDetails userDetails)
        {
            await DeleteBaseRepo(userDetails);
        }
    }
}
