using AutoMapper;
using DataModel;
using SwitDish.Domain.Contract;
using SwitDish.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain
{
    public abstract class ServiceBase
    {
        protected SwitDishDatabaseEntities DbContext { get; }

        protected ServiceBase(SwitDishDatabaseEntities dbContext)
        {
            this.DbContext = dbContext;
        }

    }
}
