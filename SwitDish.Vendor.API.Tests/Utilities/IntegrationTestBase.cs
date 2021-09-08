using NUnit.Framework;
using SwitDish.DataModel_OLD.Models;
using SwitDish.Vendor.API.Tests.DataArranger;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Vendor.API.Tests.Utilities
{
    public class IntegrationTestBase : TestBase
    {
        private Lazy<SwitDishDatabaseContext> setupDbContext;
        private Lazy<SwitDishDatabaseContext> actionDbContext;
        private Lazy<SwitDishDatabaseContext> assertDbContext;

        /// <summary>
        /// Gets the SETUP database context.
        /// </summary>
        /// <value>
        /// The SETUP database context.
        /// </value>
        protected SwitDishDatabaseContext SetupDbContext => this.setupDbContext.Value;

        /// <summary>
        /// Gets the SETUP database context.
        /// </summary>
        /// <value>
        /// The SETUP database context.
        /// </value>
        protected SwitDishDatabaseContext ActionDbContext => this.actionDbContext.Value;

        /// <summary>
        /// Gets the ASSERT database context.
        /// </summary>
        /// <value>
        /// The ASSERT database context.
        /// </value>
        protected SwitDishDatabaseContext AssertDbContext => this.assertDbContext.Value;

        /// <summary>
        /// Called at start of each test, creates a fresh DbContext and cleans out any saved error from before
        /// </summary>
        [SetUp]
        public void IntegrationTestsBaseSetUp()
        {
            DataArrangerBase.ResetSavedError();

            this.setupDbContext = new Lazy<SwitDishDatabaseContext>(CreateDbContext);
            this.actionDbContext = new Lazy<SwitDishDatabaseContext>(CreateDbContext);
            this.assertDbContext = new Lazy<SwitDishDatabaseContext>(CreateDbContext);
        }

        /// <summary>
        /// Called at end of each test, tears down the DbContext and reports any error caught in the arrangers' dispose methods
        /// Integrations the tests base tear down.
        /// </summary>
        [TearDown]
        public void IntegrationTestsBaseTearDown()
        {
            if (this.setupDbContext != null && this.setupDbContext.IsValueCreated)
            {
                this.setupDbContext.Value.Dispose();
                this.setupDbContext = null;
            }

            if (this.actionDbContext != null && this.actionDbContext.IsValueCreated)
            {
                this.actionDbContext.Value.Dispose();
                this.actionDbContext = null;
            }

            if (this.assertDbContext != null && this.assertDbContext.IsValueCreated)
            {

                this.assertDbContext.Value.Dispose();
                this.assertDbContext = null;
            }

            if (DataArrangerBase.HasError)
            {
                throw new AggregateException("These exceptions were detected during cleanup", DataArrangerBase.SavedExceptions);
            }
        }

        /// <summary>
        /// Creates a new database context.
        /// </summary>
        /// <returns>A new database context.</returns>
        protected static SwitDishDatabaseContext CreateDbContext()
        {
            // added comment to test angular deployment
            return new Database.LoggingDbContext();
        }
    }
}
