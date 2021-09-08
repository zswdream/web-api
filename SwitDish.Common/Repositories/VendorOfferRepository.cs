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
    public class VendorOfferRepository : IRepository<VendorOffer>
    {
        private SwitDishDbContext dbContext { get; }
        public VendorOfferRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<VendorOffer> GetAsync(int id)
        {
            return await this.dbContext.VendorOffers.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<VendorOffer>> GetAllAsync()
        {
            return await this.dbContext.VendorOffers.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(VendorOffer entity)
        {
            this.dbContext.VendorOffers.Add(entity);
        }

        public void Update(VendorOffer entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(VendorOffer entity)
        {
            this.dbContext.VendorOffers.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<VendorOffer>> GetAsync(Expression<Func<VendorOffer, bool>> filter = null, Func<IQueryable<VendorOffer>, IOrderedQueryable<VendorOffer>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<VendorOffer> query = dbContext.VendorOffers;

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
