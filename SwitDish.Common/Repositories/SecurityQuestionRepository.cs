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
    public class SecurityQuestionRepository : IRepository<SecurityQuestion>
    {
        private SwitDishDbContext dbContext { get; }
        public SecurityQuestionRepository(SwitDishDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SecurityQuestion> GetAsync(int id)
        {
            return await this.dbContext.SecurityQuestions.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SecurityQuestion>> GetAllAsync()
        {
            return await this.dbContext.SecurityQuestions.ToListAsync().ConfigureAwait(false);
        }

        public void Insert(SecurityQuestion entity)
        {
            this.dbContext.SecurityQuestions.Add(entity);
        }

        public void Update(SecurityQuestion entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(SecurityQuestion entity)
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

        public async Task<IEnumerable<SecurityQuestion>> GetAsync(Expression<Func<SecurityQuestion, bool>> filter = null, Func<IQueryable<SecurityQuestion>, IOrderedQueryable<SecurityQuestion>> orderBy = null, string includeProperties = "", bool tracking = true)
        {
            IQueryable<SecurityQuestion> query = dbContext.SecurityQuestions;

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
