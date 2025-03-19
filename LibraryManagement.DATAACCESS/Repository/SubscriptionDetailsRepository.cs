using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DATAACCESS.Repository
{
    
       public interface ISubscriptionDetailsRepository : IRepositoryBase<SubscriptionDetails>
    {
        Task<IEnumerable<SubscriptionDetails>> GetAllSubscriptionDetailsRepo();
        Task<SubscriptionDetails> GetSubscriptionDetailsByIdRepo(int planId);
        Task<SubscriptionDetails> CreateSubscriptionDetailsRepo(SubscriptionDetails subscriptionDetails);
        Task UpdateSubscriptionDetailsRepo(SubscriptionDetails subscriptionDetails);
        Task DeleteSubscriptionDetailsRepo(SubscriptionDetails subscriptionDetails);
    }
    public class SubscriptionDetailsRepository : RepositoryBase<SubscriptionDetails>, ISubscriptionDetailsRepository
    {
        public SubscriptionDetailsRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<SubscriptionDetails>> GetAllSubscriptionDetailsRepo()
        {
            return await FindAllBaseRepo()
                .Include(e => e.Subscription)
                .Include(e => e.User)
            .OrderByDescending(e => e.SubscriptionId)
            .ToListAsync();
        }

        public async Task<SubscriptionDetails> GetSubscriptionDetailsByIdRepo(int subscriptionId)
        {
            return await FindByConditionBaseRepo(e => e.SubscriptionId.Equals(subscriptionId)).Include(e => e.Subscription)
                .Include(e => e.User)
            .FirstOrDefaultAsync();
        }

        public async Task<SubscriptionDetails> CreateSubscriptionDetailsRepo(SubscriptionDetails subscriptionDetails)
        {
            return await CreateBaseRepo(subscriptionDetails);
        }
        public async Task UpdateSubscriptionDetailsRepo(SubscriptionDetails subscriptionDetails)
        {
            await UpdateBaseRepo(subscriptionDetails);
        }

        public async Task DeleteSubscriptionDetailsRepo(SubscriptionDetails subscriptionDetails)
        {
            await DeleteBaseRepo(subscriptionDetails);
        }
    }
}
