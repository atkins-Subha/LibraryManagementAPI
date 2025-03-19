using LibraryManagement.DATAACCESS.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace LibraryManagement.DATAACCESS.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected LibraryManagementDbContext _dbcontext { get; set; }

        public RepositoryBase(LibraryManagementDbContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public IQueryable<T> FindAllBaseRepo()
        {
            return this._dbcontext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByConditionBaseRepo(Expression<Func<T, bool>> expression)
        {
            return this._dbcontext.Set<T>().Where(expression).AsNoTracking();
        }
        public async Task<T> CreateBaseRepo(T entity)
        {
            try
            {
                var newentity = this._dbcontext.Set<T>().Add(entity);
                await this._dbcontext.SaveChangesAsync();
                return newentity.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        //public async Task UpdateBaseRepo(T entity)
        //{
        //    this._dbcontext.Set<T>().Update(entity);
        //    await this._dbcontext.SaveChangesAsync();
        //    this._dbcontext.Entry(entity).State = EntityState.Detached;

        //}

        public async Task UpdateBaseRepo(T entity)
        {
            var entry = _dbcontext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbcontext.Set<T>().Attach(entity);
            }
            entry.State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
            _dbcontext.Entry(entity).State = EntityState.Detached;
        }
        public async Task DeleteBaseRepo(T entity)
        {
            try
            {
                this._dbcontext.Set<T>().Remove(entity);
                await this._dbcontext.SaveChangesAsync();
            }
            catch(Exception ex){ throw ex; }
          
        }
    }
}
