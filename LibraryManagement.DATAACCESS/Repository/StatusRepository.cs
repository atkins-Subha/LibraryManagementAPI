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
    public interface IStatusRepository : IRepositoryBase<Status>
    {
        Task<IEnumerable<Status>> GetAllStatusRepo();
        Task<Status> GetStatusByIdRepo(int statusId);
        Task<Status> CreateStatusRepo(Status status);
        Task UpdateStatusRepo(Status status);
        Task DeleteStatusRepo(Status status);
    }
    public class StatusRepository : RepositoryBase<Status>, IStatusRepository
    {
        public StatusRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<Status>> GetAllStatusRepo()
        {
            return await FindAllBaseRepo()
            .OrderByDescending(e => e.StatusId)
            .ToListAsync();
        }

        public async Task<Status> GetStatusByIdRepo(int statusId)
        {
            return await FindByConditionBaseRepo(e => e.StatusId.Equals(statusId))
            .FirstOrDefaultAsync();
        }

        public async Task<Status> CreateStatusRepo(Status status)
        {
            return await CreateBaseRepo(status);
        }
        public async Task UpdateStatusRepo(Status status)
        {
            await UpdateBaseRepo(status);
        }

        public async Task DeleteStatusRepo(Status status)
        {
            await DeleteBaseRepo(status);
        }
    }
}
