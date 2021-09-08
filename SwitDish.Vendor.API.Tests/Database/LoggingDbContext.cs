using SwitDish.DataModel_OLD.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Vendor.API.Tests.Database
{
    public class LoggingDbContext : SwitDishDatabaseContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="userId">The user identifier.</param>
        public LoggingDbContext()
        {
            // override the base class; always make Database.Log point at a real fUnction
            // this.Database.Log = WriteLogLineInternal;
        }

        ///// <summary>
        ///// Logs entity validation errors from Entity Framework to the console.
        ///// </summary>
        ///// <param name="dbEntityValidationException">The database entity validation exception.</param>
        //public static void WriteLogValidationErrors(DbEntityValidationException dbEntityValidationException)
        //{
        //    WriteLogLineInternal(PrintableEntityValidationErrors(dbEntityValidationException));
        //}

        ///// <summary>
        ///// Logs an sql command in tests in all builds
        ///// </summary>
        ///// <remarks>
        ///// hides the debug-build-only impl in the base class
        ///// </remarks>
        ///// <param name="cmd">The command.</param>
        //public static new void DebugLogSqlCommand(SqlCommand cmd)
        //{
        //    WriteLogSqlCommand(cmd);
        //}

        ///// <summary>
        ///// Logs an sql string in debug builds only
        ///// </summary>
        ///// <param name="sql">The SQL.</param>
        ///// <remarks>
        ///// hides the debug-build-only impl in the base class
        ///// </remarks>
        //public static new void DebugLogLine(string sql)
        //{
        //    WriteLogLineInternal(sql);
        //}

        ///// <summary>
        ///// Logs an sql string in debug builds only
        ///// </summary>
        ///// <param name="sql">The SQL.</param>
        ///// <remarks>
        ///// hides the debug-build-only impl in the base class
        ///// </remarks>
        //public static new void DebugLog(string sql)
        //{
        //    WriteLogInternal(sql);
        //}

        ///// <summary>
        ///// Logs the number of rows affected by the last SQL statement.
        ///// </summary>
        ///// <remarks>
        ///// Logs to the debug stream so we can see it in the debugger as it's created
        ///// </remarks>
        ///// <param name="numberOfRows">
        ///// The number of rows affected.
        ///// </param>
        //public static void DebugLogSqlRowsAffected(int numberOfRows)
        //{
        //    WriteLogLineInternal(
        //        numberOfRows == 0 ? "no rows affected" :
        //        numberOfRows == 1 ? "1 row affected" : $"{numberOfRows} rows affected");
        //}

        ///// <summary>
        ///// Reloads the entity, overwriting any property values with values from the database.
        ///// All the entity needs is for the primary key to be populated.
        ///// The entity will be in the Unchanged state after calling this method.
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <param name="entity">The entity.</param>
        //public TEntity ReloadEntity<TEntity>(TEntity entity)
        //    where TEntity : class
        //{
        //    DbEntityEntry<TEntity> dbEntityEntry = this.Entry(entity);

        //    if (dbEntityEntry.State == EntityState.Detached)
        //    {
        //        this.Set(typeof(TEntity)).Attach(entity);
        //    }

        //    dbEntityEntry.Reload();
        //    return entity;
        //}

        ///// <summary>
        ///// Loads a reference navigation property.
        ///// Note that if the entity already exists in the context, then it will not overwritten with values from the database.
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <typeparam name="TProperty"> The type of the property. </typeparam>
        ///// <param name="entity">The entity.</param>
        ///// <param name="navigationProperty"> An expression representing the navigation property.</param>
        ///// <example>
        ///// dbContext.LoadReference(lead, l => l.tblFinance);
        ///// </example>
        //public TProperty LoadReference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> navigationProperty)
        //    where TEntity : class
        //    where TProperty : class
        //{
        //    DbReferenceEntry<TEntity, TProperty> dbReferenceEntry = this.Entry(entity).Reference(navigationProperty);
        //    dbReferenceEntry.Load();
        //    return dbReferenceEntry.CurrentValue;
        //}

        ///// <summary>
        ///// Reloads a reference navigation property from the database overwriting any property values with values from the database.
        ///// The reference entity will be in the Unchanged state after calling this method.
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <typeparam name="TProperty"> The type of the property. </typeparam>
        ///// <param name="entity">The entity.</param>
        ///// <param name="navigationProperty"> An expression representing the navigation property.</param>
        ///// <example>
        ///// dbContext.LoadReference(lead, l => l.tblFinance);
        ///// </example>
        //public TProperty ReloadReference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> navigationProperty)
        //    where TEntity : class
        //    where TProperty : class
        //{
        //    var property = this.LoadReference(entity, navigationProperty);
        //    return property != null ? this.ReloadEntity(property) : null;
        //}

        ///// <summary>
        ///// Loads a container navigation property.
        ///// Note that entities that already exist in the context are not overwritten with values from the database.
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <typeparam name="TElement"> The type of elements in the collection.</typeparam>
        ///// <param name="entity">The entity.</param>
        ///// <param name="navigationProperty"> An expression representing the navigation property.</param>
        ///// <example>
        ///// dbContext.LoadContainer(lead, l => l.tblLeadClients);
        ///// </example>
        //public ICollection<TElement> LoadContainer<TEntity, TElement>(
        //    TEntity entity,
        //    Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
        //    where TEntity : class
        //    where TElement : class
        //{
        //    DbCollectionEntry<TEntity, TElement> dbCollectionEntry = this.Entry(entity).Collection(navigationProperty);
        //    dbCollectionEntry.Load();
        //    return dbCollectionEntry.CurrentValue;
        //}

        ///// <summary>
        ///// Reloads a container navigation property by clearing it out then calling load
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <typeparam name="TElement"> The type of elements in the collection.</typeparam>
        ///// <param name="entity">The entity.</param>
        ///// <param name="navigationProperty"> An expression representing the navigation property.</param>
        ///// <example>
        ///// dbContext.ReloadContainer(lead, l => l.tblLeadClients);
        ///// </example>
        //public ICollection<TElement> ReloadContainer<TEntity, TElement>(
        //    TEntity entity,
        //    Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
        //    where TEntity : class
        //    where TElement : class
        //{
        //    // compile the expression into a lambda that can be executed
        //    Func<TEntity, ICollection<TElement>> navigationSelector = navigationProperty.Compile();

        //    // Detach child elements from the context, forcing them to be reloaded on the next query
        //    List<TElement> collectionList = navigationSelector(entity).ToList();

        //    foreach (TElement child in collectionList)
        //    {
        //        this.Entry(child).State = EntityState.Detached;
        //    }

        //    // then load them again
        //    return this.LoadContainer(entity, navigationProperty);
        //}
    }
}
