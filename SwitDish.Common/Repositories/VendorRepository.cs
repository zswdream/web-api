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
    public class VendorRepository : IRepository<Vendor>
    {
        private SwitDishDbContext dbContext { get; }
        public VendorRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Vendor> GetAsync(int id)
        {
            return await this.dbContext.Vendors.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Vendor>> GetAllAsync()
        {
            return await this.dbContext.Vendors.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(Vendor entity)
        {
            this.dbContext.Vendors.Add(entity);
        }

        public void Update(Vendor entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Vendor entity)
        {
            this.dbContext.Vendors.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<Vendor>> GetAsync(Expression<Func<Vendor, bool>> filter = null, Func<IQueryable<Vendor>, IOrderedQueryable<Vendor>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<Vendor> query = dbContext.Vendors;

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
