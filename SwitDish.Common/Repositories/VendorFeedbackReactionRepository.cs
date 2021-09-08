using Microsoft.EntityFrameworkCore;
using SwitDish.Common.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SwitDish.Common.Repositories
{
    public class VendorFeedbackReactionRepository : IRepository<VendorFeedbackReaction>
    {
        private SwitDishDbContext dbContext { get; }
        public VendorFeedbackReactionRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<VendorFeedbackReaction> GetAsync(int id)
        {
            return await this.dbContext.VendorFeedbackReactions.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<VendorFeedbackReaction>> GetAllAsync()
        {
            return await this.dbContext.VendorFeedbackReactions.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(VendorFeedbackReaction entity)
        {
            this.dbContext.VendorFeedbackReactions.Add(entity);
        }

        public void Update(VendorFeedbackReaction entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(VendorFeedbackReaction entity)
        {
            this.dbContext.VendorFeedbackReactions.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<VendorFeedbackReaction>> GetAsync(Expression<Func<VendorFeedbackReaction, bool>> filter = null, Func<IQueryable<VendorFeedbackReaction>, IOrderedQueryable<VendorFeedbackReaction>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<VendorFeedbackReaction> query = dbContext.VendorFeedbackReactions;

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
