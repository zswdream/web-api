using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SwitDish.DataModel_OLD.Models;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SwitDish.Vendor.API.Tests.DataArranger
{
    public class DataArranger<T>
        : DataArrangerBase,
          IDisposable
        where T : class
    {
        private readonly DbSet<T> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataArranger{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entity">The data object.</param>
        /// <param name="createFlag">The create flag.</param>
        /// <exception cref="ArgumentNullException">
        /// dbContext
        /// or
        /// entity
        /// </exception>
        [SuppressMessage(
            "Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification = "Everything the Create needs is set up by the point it is called")]
        protected DataArranger(SwitDishDatabaseContext dbContext, T entity, Behaviour createFlag = Behaviour.TakeOwnership)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            this.dbSet = this.DbContext.Set<T>();

            if (createFlag == Behaviour.CreateEntity)
            {
                // Create the entity in the using statement and call SaveChanges so it has a populated primary key
                // ReSharper disable once VirtualMemberCallInConstructor
                this.Create();
            }

            if (createFlag == Behaviour.ReloadEntity)
            {
                // Locate the entity in the using statement and call SaveChanges so it has a populated primary key
                this.Reload();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DataArranger{T}"/> class.
        /// </summary>
        ~DataArranger()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the name of the entity type.
        /// </summary>
        /// <value>
        /// The name of the entity type.
        /// </value>
        public string EntityName => typeof(T).Name;

        /// <summary>
        /// Gets the contained entity object as a type from the domain model. Useful for when the
        /// implicit conversion isn't chosen automatically
        /// </summary>
        /// <value>The data object.</value>
        public T Entity
        {
            get; private set;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected SwitDishDatabaseContext DbContext
        {
            get;
        }

        /// <summary>
        /// Gets a real SqlServer connection from the DbContext
        /// </summary>
        /// <value>
        /// The SQL connection.
        /// </value>
        /// <exception cref="InvalidOperationException">sqlConnection: not on a sqlserver database</exception>
        protected SqlConnection SqlConnection
        {
            get
            {
                if (this.DbContext.Database.GetDbConnection() is SqlConnection sqlConnection)
                {
                    return sqlConnection;
                }

                throw new InvalidOperationException("SqlConnection: not connected to a sql server database");
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DataArranger{T}"/>.
        /// </summary>
        /// <param name="arranger">The arranger.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage(
            "Microsoft.Usage",
            "CA2225:OperatorOverloadsHaveNamedAlternates",
            Justification = "We want to use an arranger as the object it's managing")]
        public static implicit operator T(DataArranger<T> arranger)
        {
            return arranger?.Entity;
        }

        /// <summary>
        /// Creates this instance in the db.
        /// </summary>
        public virtual void Create()
        {
            if (this.Entity == null)
            {
                throw new NoNullAllowedException($"can't create {this}");
            }

            this.dbSet.Add(this.Entity);

            this.DbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes this instance from the db.
        /// </summary>
        public virtual void Delete()
        {
            if (this.Entity == null)
            {
                throw new NoNullAllowedException($"can't delete {this}");
            }

            // the attach ensures the item is known about by EF, so it will definitely generate the delete
            this.dbSet.Attach(this.Entity);
            this.dbSet.Remove(this.Entity);
            this.DbContext.SaveChanges();
        }

        /// <summary>
        /// just tell EF not to worry about this entity any more, so it can clean up any collections it might have been in
        /// </summary>
        public void MarkDiscarded()
        {
            var entry = this.DbContext.Entry(this.Entity);
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Detached;
                    break;
                default:
                    entry.State = EntityState.Detached;
                    break;
            }
        }

        /// <summary>
        /// Count the number of these entities in the db, SELECT COUNT(*) FROM TABLE_NAME
        /// </summary>
        /// <returns>The count in the db</returns>
        public virtual int Count()
        {
            return this.dbSet.Count();
        }

        /// <summary>
        /// Dispose an instance of the <see cref="DataArranger{T}"/> class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a string that represents the current object. This will be better if the domain
        /// object itself implements ToString, or the derived arranger could override it to provide
        /// something useful too.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Entity == null
                ? $"{this.GetType().Name} for null {this.EntityName}"
                : $"{this.GetType().Name} for {this.Entity}";
        }

        /// <summary>
        /// Reloads this entity from the database.
        /// </summary>
        /// <remarks>
        /// Accessing the associations may cause additional lazy-load sql queries, but you might still have to load them explicitly
        /// </remarks>
        public void Reload()
        {
            //// this.DbContext.ReloadEntity(this.Entity);
        }

        /// <summary>
        /// Dispose and finalize an instance of the <see cref="DataArranger{T}"/> class
        /// </summary>
        /// <param name="disposing">run dispose-only logic too</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.Entity != null)
            {
                if (disposing)
                {
                    if (this.DbContext != null)
                    {
                        try
                        {
                            this.Delete();
                        }
                        catch (Exception e)
                        {
                            string title = $"dispose: error deleting {this}";

                            // we must not throw from a dispose
                            // so silently log the error so and rethrow it later
                            LogErrorForLater(title, e);
                        }
                    }
                }

                this.Entity = null;
            }
        }

        /// <summary>
        /// Opens the connection and runs the action, then closes it again.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void ExecuteOnOpenConnection(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            bool startedClosed = this.DbContext.Database.GetDbConnection().State != ConnectionState.Open;

            try
            {
                if (startedClosed)
                {
                    this.DbContext.Database.OpenConnection();
                }

                action();
            }
            finally
            {
                if (startedClosed && this.DbContext.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    this.DbContext.Database.CloseConnection();
                }
            }
        }
    }
}
