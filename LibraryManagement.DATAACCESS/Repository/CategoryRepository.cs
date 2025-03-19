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
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoryRepo();
        Task<Category> GetCategoryByIdRepo(int categoryId);
        Task<Category> CreateCategoryRepo(Category category);
        Task UpdateCategoryRepo(Category category);
        Task DeleteCategoryRepo(Category category);
    }
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<Category>> GetAllCategoryRepo()
        {
            return await FindAllBaseRepo()
            .OrderByDescending(e => e.CategoryId)
            .ToListAsync();
        }
        public async Task<Category> GetCategoryByIdRepo(int categoryId)
        {
            return await FindByConditionBaseRepo(e => e.CategoryId.Equals(categoryId))
            .FirstOrDefaultAsync();
        }

        public async Task<Category> CreateCategoryRepo(Category category)
        {
            return await CreateBaseRepo(category);
        }
        public async Task UpdateCategoryRepo(Category category)
        {
            await UpdateBaseRepo(category);
        }

        public async Task DeleteCategoryRepo(Category category)
        {
            await DeleteBaseRepo(category);
        }
    }
}
