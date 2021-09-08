using Microsoft.Data.SqlClient;
using SwitDish.DataModel_OLD.Models;
using System;
using System.Linq;
using System.Text;

namespace SwitDish.Vendor.API.Tests.DataArranger
{
    public class AddressArranger : DataArranger<Address>
    {
        private readonly SwitDishDatabaseContext dbContext;

        private readonly Address address;

        public AddressArranger(SwitDishDatabaseContext dbContext, Address address, Behaviour createFlag = Behaviour.TakeOwnership)
            : base(dbContext, address, createFlag)
        {
            this.dbContext = dbContext;
            this.address = address;
        }

        public static Address DefaultTestData()
        {
            return new Address
            {
                BuildingNumber = "17",
                AddressLine1 = "Manor House",
                AddressLine2 = "",
                PostCode = "N17 OPG",
                Country = "United Kingdom"
            };
        }

        public bool ExistsByBusinessKey()
        {
            var addressEntity = this.dbContext.
                Addresses
                .Where(d=> d.AddressId.Equals(this.address))
                .FirstOrDefault();

            if (addressEntity == null)
            {
                return false;
            }

            return Convert.ToInt32(addressEntity.AddressId) != 0;
        }
    }
}
