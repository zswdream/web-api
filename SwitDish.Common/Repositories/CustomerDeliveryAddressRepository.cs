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
    public class CustomerDeliveryAddressRepository : IRepository<CustomerDeliveryAddress>
    {
        private SwitDishDbContext dbContext { get; }
        public CustomerDeliveryAddressRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomerDeliveryAddress> GetAsync(int id)
        {
            return await this.dbContext.CustomerDeliveryAddresses.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerDeliveryAddress>> GetAllAsync()
        {
            return await this.dbContext.CustomerDeliveryAddresses.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(CustomerDeliveryAddress entity)
        {
            this.dbContext.CustomerDeliveryAddresses.Add(entity);
        }

        public void Update(CustomerDeliveryAddress entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(CustomerDeliveryAddress entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<CustomerDeliveryAddress>> GetAsync(Expression<Func<CustomerDeliveryAddress, bool>> filter = null, Func<IQueryable<CustomerDeliveryAddress>, IOrderedQueryable<CustomerDeliveryAddress>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<CustomerDeliveryAddress> query = dbContext.CustomerDeliveryAddresses;

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
