using SwitDish.DataModel_OLD.Models;
using System.Linq;

namespace SwitDish.Vendor.API.Tests.DataArranger
{
    public class CompanyArranger : DataArranger<Company>
    {
        private readonly SwitDishDatabaseContext dbContext;

        private readonly Company company;

        public CompanyArranger(SwitDishDatabaseContext dbContext, Company company, Behaviour createFlag = Behaviour.TakeOwnership)
            : base(dbContext, company, createFlag)
        {
            this.dbContext = dbContext;
            this.company = company;
        }

        public static Company DefaultTestData()
        {
            return new Company
            {
                Name = "Test company",
                Description = "Test "
            };
        }

        public static Company CreateTestData(string name, string description)
        {
            return new Company
            {
                Name = name,
                Description = description
            };
        }

        public bool ExistsByBusinessKey()
        {
            var companyEntity = this.dbContext.Companies.Where(d=>d.CompanyId.Equals(this.company))
                .FirstOrDefault();

            if (companyEntity == null)
            {
                return false;
            }

            return companyEntity.CompanyId != 0;
        }
    }
}
