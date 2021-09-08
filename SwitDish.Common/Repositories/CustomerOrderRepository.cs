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
    public class CustomerOrderRepository : IRepository<CustomerOrder>
    {
        private SwitDishDbContext dbContext { get; }
        public CustomerOrderRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomerOrder> GetAsync(int id)
        {
            return await this.dbContext.CustomerOrders.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerOrder>> GetAllAsync()
        {
            return await this.dbContext.CustomerOrders.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(CustomerOrder entity)
        {
            this.dbContext.CustomerOrders.Add(entity);
        }

        public void Update(CustomerOrder entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(CustomerOrder entity)
        {
            this.dbContext.CustomerOrders.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<CustomerOrder>> GetAsync(Expression<Func<CustomerOrder, bool>> filter = null, Func<IQueryable<CustomerOrder>, IOrderedQueryable<CustomerOrder>> CustomerOrderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<CustomerOrder> query = dbContext.CustomerOrders;

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