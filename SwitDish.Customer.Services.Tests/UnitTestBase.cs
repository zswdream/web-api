using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using SwitDish.Common.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwitDish.Customer.Services.Tests
{
    public class UnitTestBase
    {
        protected readonly IMapper mapper;
        protected readonly ILoggerManager logger;
        protected readonly IWebHostEnvironment webHostEnvironment;
        protected readonly DbContextOptions<SwitDishDbContext> dbOptions;
        protected SwitDishDbContext dbContext;
        public UnitTestBase()
        {
            // Arrange In Memory SQLite DbOptions
            this.dbOptions = new DbContextOptionsBuilder<DataModel.Models.SwitDishDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlite("DataSource =:memory:").Options;

            // Arrange Common Mapper and include required Mapping Profiles
            this.mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.AddProfile<SwitDish.Services.Mappings.MappingProfile>();
            }));

            // Arrange Common Logger
            this.logger = new Common.Logger.LoggerManager();
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.dbContext.ChangeTracker.Entries().ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
