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
    public class VendorGalleryImageRepository : IRepository<VendorGalleryImage>
    {
        private SwitDishDbContext dbContext { get; }
        public VendorGalleryImageRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<VendorGalleryImage> GetAsync(int id)
        {
            return await this.dbContext.VendorGalleryImages.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<VendorGalleryImage>> GetAllAsync()
        {
            return await this.dbContext.VendorGalleryImages.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(VendorGalleryImage entity)
        {
            this.dbContext.VendorGalleryImages.Add(entity);
        }

        public void Update(VendorGalleryImage entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(VendorGalleryImage entity)
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
        public async Task<IEnumerable<VendorGalleryImage>> GetAsync(Expression<Func<VendorGalleryImage, bool>> filter = null, Func<IQueryable<VendorGalleryImage>, IOrderedQueryable<VendorGalleryImage>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<VendorGalleryImage> query = dbContext.VendorGalleryImages;

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
