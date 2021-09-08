using Microsoft.EntityFrameworkCore;
using SwitDish.Common.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Common.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private SwitDishDbContext dbContext { get; }
        public CustomerRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Customer> GetAsync(int id)
        {
            return await this.dbContext.Customers.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await this.dbContext.Customers.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(Customer entity)
        {
            this.dbContext.Customers.Add(entity);
        }

        public void Update(Customer entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Customer entity)
        {
            this.dbContext.ChangeTracker.Entries()
                .ToList()
                .ForEach(d => d.State = EntityState.Detached);

            this.dbContext.Customers.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<Customer>> GetAsync(Expression<Func<Customer, bool>> filter = null, Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<Customer> query = dbContext.Customers;

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