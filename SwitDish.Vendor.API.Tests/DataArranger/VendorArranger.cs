using SwitDish.DataModel_OLD.Models;
using System.Linq;

namespace SwitDish.Vendor.API.Tests.DataArranger
{
    public class VendorArranger : DataArranger<DataModel_OLD.Models.Vendor>
    {
        private readonly SwitDishDatabaseContext dbContext;

        private readonly DataModel_OLD.Models.Vendor vendor;

        public VendorArranger(SwitDishDatabaseContext dbContext, DataModel_OLD.Models.Vendor vendor, Behaviour createFlag = Behaviour.TakeOwnership)
            : base(dbContext, vendor, createFlag)
        {
            this.dbContext = dbContext;
            this.vendor = vendor;
        }

        public static DataModel_OLD.Models.Vendor DefaultTestData()
        {
            return new DataModel_OLD.Models.Vendor
            {
                Name = "Test Vendor"
            };
        }

        public static DataModel_OLD.Models.Vendor CreateTestData(string vendorName, AddressArranger addressArranger)
        {
            return new DataModel_OLD.Models.Vendor
            {
                Name = vendorName,
                AddressId = addressArranger.Entity.AddressId
            };
        }

        public bool ExistsByBusinessKey()
        {
            var agentEntity = this.dbContext.Vendors.Where(d=> d.VendorId.Equals(this.vendor))
                .FirstOrDefault();

            if (agentEntity == null)
            {
                return false;
            }

            return agentEntity.VendorId != 0;
        }
    }
}
