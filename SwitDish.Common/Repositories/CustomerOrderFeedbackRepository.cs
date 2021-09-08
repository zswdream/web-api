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
    public class CustomerOrderFeedbackRepository : IRepository<CustomerOrderFeedback>
    {
        private SwitDishDbContext dbContext { get; }
        public CustomerOrderFeedbackRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomerOrderFeedback> GetAsync(int id)
        {
            return await this.dbContext.CustomerOrderFeedbacks.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerOrderFeedback>> GetAllAsync()
        {
            return await this.dbContext.CustomerOrderFeedbacks.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(CustomerOrderFeedback entity)
        {
            this.dbContext.CustomerOrderFeedbacks.Add(entity);
        }

        public void Update(CustomerOrderFeedback entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(CustomerOrderFeedback entity)
        {
            this.dbContext.CustomerOrderFeedbacks.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<CustomerOrderFeedback>> GetAsync(Expression<Func<CustomerOrderFeedback, bool>> filter = null, Func<IQueryable<CustomerOrderFeedback>, IOrderedQueryable<CustomerOrderFeedback>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<CustomerOrderFeedback> query = dbContext.CustomerOrderFeedbacks;

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