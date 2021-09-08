//using AutoMapper;
//using DataModel;
//using DataModel.Contract;
//using SwitDish.Domain.Contract;
//using SwitDish.Domain.Enum;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SwitDish.Domain
//{
//    public abstract class ServiceBase1<TDatabaseEntity, TDomainEntity>
//        where TDatabaseEntity : class, IBaseDatabaseEntity
//        where TDomainEntity : class, IBaseDomainEntity
//    {
//        protected SwitDishDatabaseEntities DbContext { get; }
//        protected IMapper mapper;

//        protected ServiceBase1(SwitDishDatabaseEntities dbContext, IMapper mapper)
//        {
//            this.DbContext = dbContext;
//            this.mapper = mapper;
//        }

//        public async Task<TDomainEntity> GetAsync(int id)
//        {
//            var dbEntity = await this.DbContext.Set<TDatabaseEntity>().FindAsync(id).ConfigureAwait(false);
//            var domainEntity = this.mapper.Map <TDomainEntity>(dbEntity);
//            return domainEntity;
//        }

//        public async Task<IList<TDomainEntity>> GetAllAsync()
//        {
//            var dbEntities = await this.DbContext.Set<TDatabaseEntity>().ToListAsync().ConfigureAwait(false);
//            var domainEntities = this.mapper.Map<IList<TDomainEntity>>(dbEntities);
//            return domainEntities;
//        }

//        public virtual async Task<TDomainEntity> CreateAsync(TDomainEntity domainEntity)
//        {
//            if (domainEntity.Id > 0)
//            {
//                throw new InvalidOperationException("The entity already has an entry");
//            }

//            var dbEntity = this.mapper.Map<TDatabaseEntity>(domainEntity);

//            this.DbContext.Set<TDatabaseEntity>().Add(dbEntity);
//            await this.DbContext.SaveChangesAsync().ConfigureAwait(false);

//            domainEntity = this.mapper.Map<TDomainEntity>(dbEntity);
//            return domainEntity;
//        }

//        public virtual async Task<TDomainEntity> UpdateAsync(TDomainEntity domainEntity)
//        {
//            if (domainEntity.Id == 0)
//            {
//                throw new InvalidOperationException("The entry does not have an existing Id");
//            }

//            var dbEntity = this.mapper.Map<TDatabaseEntity>(domainEntity);

//            this.ModifyEntityWithoutTracking(dbEntity, domainEntity.Id, DbAction.Update);
//            await this.DbContext.SaveChangesAsync().ConfigureAwait(false);

//            domainEntity = this.mapper.Map<TDomainEntity>(dbEntity);
//            return domainEntity;
//        }

//        private void ModifyEntityWithoutTracking (TDatabaseEntity dbEntity, int id, DbAction dbAction)
//        {
//            var local = this.DbContext.Set<TDatabaseEntity>().Local.FirstOrDefault(entry => entry.Id == id);
//            if (local?.Id > 0)
//            {
//                this.DbContext.Entry(local).State = EntityState.Detached;
//            }

//            if (dbAction == DbAction.Update)
//            {
//                this.DbContext.Entry(dbEntity).State = EntityState.Modified;
//            }

//            if (dbAction == DbAction.Delete)
//            {
//                this.DbContext.Entry(dbEntity).State = EntityState.Deleted;
//            }
//        }
//    }
//}
