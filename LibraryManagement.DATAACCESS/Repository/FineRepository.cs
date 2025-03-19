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
    public interface IFineRepository : IRepositoryBase<Fine>
    {
        Task<IEnumerable<Fine>> GetAllFineRepo();
        Task<Fine> GetFineByIdRepo(int fineId);
        Task<Fine> CreateFineRepo(Fine fine);
        Task UpdateFineRepo(Fine fine);
        Task DeleteFineRepo(Fine fine);
    }
    public class FineRepository : RepositoryBase<Fine>, IFineRepository
    {
        public FineRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<Fine>> GetAllFineRepo()
        {
            return await FindAllBaseRepo()
            .OrderByDescending(e => e.FineId)
            .ToListAsync();
        }

        public async Task<Fine> GetFineByIdRepo(int fineId)
        {
            return await FindByConditionBaseRepo(e => e.FineId.Equals(fineId))
            .FirstOrDefaultAsync();
        }

        public async Task<Fine> CreateFineRepo(Fine Fine)
        {
            return await CreateBaseRepo(Fine);
        }
        public async Task UpdateFineRepo(Fine Fine)
        {
            await UpdateBaseRepo(Fine);
        }

        public async Task DeleteFineRepo(Fine Fine)
        {
            await DeleteBaseRepo(Fine);
        }
    }
}
