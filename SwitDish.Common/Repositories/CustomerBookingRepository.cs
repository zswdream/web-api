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
    public class CustomerBookingRepository : IRepository<CustomerBooking>
    {
        private SwitDishDbContext dbContext { get; }
        public CustomerBookingRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomerBooking> GetAsync(int id)
        {
            return await this.dbContext.CustomerBookings.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerBooking>> GetAllAsync()
        {
            return await this.dbContext.CustomerBookings.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(CustomerBooking entity)
        {
            this.dbContext.CustomerBookings.Add(entity);
        }

        public void Update(CustomerBooking entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(CustomerBooking entity)
        {
            this.dbContext.CustomerBookings.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<CustomerBooking>> GetAsync(Expression<Func<CustomerBooking, bool>> filter = null, Func<IQueryable<CustomerBooking>, IOrderedQueryable<CustomerBooking>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<CustomerBooking> query = dbContext.CustomerBookings;

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
