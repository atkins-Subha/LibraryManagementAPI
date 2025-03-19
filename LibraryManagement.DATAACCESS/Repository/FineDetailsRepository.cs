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
    public interface IFineDetailsRepository : IRepositoryBase<FineDetails>
    {
        Task<IEnumerable<FineDetails>> GetAllFineDetailsRepo();
        Task<FineDetails> GetFineDetailsByIdRepo(int fineId);
        Task<FineDetails> CreateFineDetailsRepo(FineDetails fineDetail);
        Task UpdateFineDetailsRepo(FineDetails fineDetail);
        Task DeleteFineDetailsRepo(FineDetails fineDetail);
    }
    public class FineDetailsRepository : RepositoryBase<FineDetails>, IFineDetailsRepository
    {
        public FineDetailsRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<FineDetails>> GetAllFineDetailsRepo()
        {
            return await FindAllBaseRepo()
             .Include(e => e.Fine)
             .Include(e => e.User)
             .Include(e => e.Book)
            .OrderByDescending(e => e.FineDetailsId)
            .ToListAsync();
        }

        public async Task<FineDetails> GetFineDetailsByIdRepo(int fineDetailsId)
        {
            return await FindByConditionBaseRepo(e => e.FineDetailsId.Equals(fineDetailsId))
            .FirstOrDefaultAsync();
        }

        public async Task<FineDetails> CreateFineDetailsRepo(FineDetails fineDetails)
        {
            return await CreateBaseRepo(fineDetails);
        }
        public async Task UpdateFineDetailsRepo(FineDetails fineDetails)
        {
            await UpdateBaseRepo(fineDetails);
        }

        public async Task DeleteFineDetailsRepo(FineDetails fineDetails)
        {
            await DeleteBaseRepo(fineDetails);
        }
    }
}
