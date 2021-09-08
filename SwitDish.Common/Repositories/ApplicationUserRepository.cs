using Microsoft.EntityFrameworkCore;
using SwitDish.Common.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Common.Repositories
{
    public class ApplicationUserRepository : IRepository<ApplicationUser>
    {
        private SwitDishDbContext dbContext { get; }
        public ApplicationUserRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ApplicationUser> GetAsync(int id)
        {
            return await this.dbContext.ApplicationUsers.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await this.dbContext.ApplicationUsers.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(ApplicationUser entity)
        {
            this.dbContext.ApplicationUsers.Add(entity);
        }

        public void Update(ApplicationUser entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(ApplicationUser entity)
        {
            this.dbContext.ApplicationUsers.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAsync(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<ApplicationUser> query = dbContext.ApplicationUsers;

            if (filter != null)
                query = query.Where(filter);

            if (!tracking)
                query = query.AsNoTracking();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);


            if (orderBy != null)
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            else
                return await query.ToListAsync().ConfigureAwait(false);
        }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
