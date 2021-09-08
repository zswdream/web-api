﻿using Microsoft.EntityFrameworkCore;
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
    public class CustomerFavouriteVendorRepository : IRepository<CustomerFavouriteVendor>
    {
        private SwitDishDbContext dbContext { get; }
        public CustomerFavouriteVendorRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomerFavouriteVendor> GetAsync(int id)
        {
            return await this.dbContext.CustomerFavouriteVendors.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerFavouriteVendor>> GetAllAsync()
        {
            return await this.dbContext.CustomerFavouriteVendors.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(CustomerFavouriteVendor entity)
        {
            this.dbContext.CustomerFavouriteVendors.Add(entity);
        }

        public void Update(CustomerFavouriteVendor entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(CustomerFavouriteVendor entity)
        {
            this.dbContext.CustomerFavouriteVendors.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<CustomerFavouriteVendor>> GetAsync(Expression<Func<CustomerFavouriteVendor, bool>> filter = null, Func<IQueryable<CustomerFavouriteVendor>, IOrderedQueryable<CustomerFavouriteVendor>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<CustomerFavouriteVendor> query = dbContext.CustomerFavouriteVendors;

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