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
    
       public interface ISubscriptionRepository : IRepositoryBase<Subscription>
    {
        Task<IEnumerable<Subscription>> GetAllSubscriptionRepo();
        Task<Subscription> GetSubscriptionByIdRepo(int subscriptionId);
        Task<Subscription> CreateSubscriptionRepo(Subscription subscription);
        Task UpdateSubscriptionRepo(Subscription subscription);
        Task DeleteSubscriptionRepo(Subscription subscription);
    }
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionRepo()
        {
            return await FindAllBaseRepo()
            //.OrderByDescending(e => e.SubscriptionId)
            .ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdRepo(int subscriptionId)
        {
            return await FindByConditionBaseRepo(e => e.SubscriptionId.Equals(subscriptionId))
            .FirstOrDefaultAsync();
        }

        public async Task<Subscription> CreateSubscriptionRepo(Subscription subscription)
        {
            return await CreateBaseRepo(subscription);
        }
        public async Task UpdateSubscriptionRepo(Subscription subscription)
        {
            await UpdateBaseRepo(subscription);
        }

        public async Task DeleteSubscriptionRepo(Subscription subscription)
        {
            await DeleteBaseRepo(subscription);
        }
    }
}
