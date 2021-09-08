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
    public class ProductCategoryRepository : IRepository<ProductCategory>
    {
        private SwitDishDbContext dbContext { get; }
        public ProductCategoryRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ProductCategory> GetAsync(int id)
        {
            return await this.dbContext.ProductCategories.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await this.dbContext.ProductCategories.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(ProductCategory entity)
        {
            this.dbContext.ProductCategories.Add(entity);
        }

        public void Update(ProductCategory entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(ProductCategory entity)
        {
            this.dbContext.ProductCategories.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<ProductCategory>> GetAsync(Expression<Func<ProductCategory, bool>> filter = null, Func<IQueryable<ProductCategory>, IOrderedQueryable<ProductCategory>> CustomerOrderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<ProductCategory> query = dbContext.ProductCategories;

            if (filter != null)
                query = query.Where(filter);

            if (!tracking)
                query = query.AsNoTracking();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);


            if (CustomerOrderBy != null)
                return await CustomerOrderBy(query).ToListAsync().ConfigureAwait(false);
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