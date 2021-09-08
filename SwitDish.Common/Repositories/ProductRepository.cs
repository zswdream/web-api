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
    public class ProductRepository : IRepository<Product>
    {
        private SwitDishDbContext dbContext { get; }
        public ProductRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Product> GetAsync(int id)
        {
            return await this.dbContext.Products.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this.dbContext.Products.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(Product entity)
        {
            this.dbContext.Products.Add(entity);
        }

        public void Update(Product entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Product entity)
        {
            this.dbContext.Products.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<Product>> GetAsync(Expression<Func<Product, bool>> filter = null, Func<IQueryable<Product>, IOrderedQueryable<Product>> CustomerOrderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<Product> query = dbContext.Products;

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