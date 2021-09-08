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
    public class CustomerSecurityQuestionRepository : IRepository<CustomerSecurityQuestion>
    {
        private SwitDishDbContext dbContext { get; }
        public CustomerSecurityQuestionRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomerSecurityQuestion> GetAsync(int id)
        {
            return await this.dbContext.CustomerSecurityQuestions.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerSecurityQuestion>> GetAllAsync()
        {
            return await this.dbContext.CustomerSecurityQuestions.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(CustomerSecurityQuestion entity)
        {
            this.dbContext.CustomerSecurityQuestions.Add(entity);
        }

        public void Update(CustomerSecurityQuestion entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(CustomerSecurityQuestion entity)
        {
            this.dbContext.CustomerSecurityQuestions.Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerSecurityQuestion>> GetAsync(Expression<Func<CustomerSecurityQuestion, bool>> filter = null, Func<IQueryable<CustomerSecurityQuestion>, IOrderedQueryable<CustomerSecurityQuestion>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<CustomerSecurityQuestion> query = dbContext.CustomerSecurityQuestions;

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