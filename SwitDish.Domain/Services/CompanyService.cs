using AutoMapper;
using DataModel;
using SwitDish.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using SwitDish.Domain.Models;

namespace SwitDish.Domain.Services
{
    public class CompanyService : ServiceBase, ICompanyService
    {


        public CompanyService(SwitDishDatabaseEntities dbContext) : base(dbContext)
        {

        }

        public async Task<Models.Company> SaveOrUpdate(Models.Company companyDetails)
        {
            try
            {
                var dbCompany = Mapper.Map<DataModel.Company>(companyDetails);
                if (companyDetails.CompanyId != 0)
                {
                    // update
                    this.DbContext.Entry(dbCompany).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    //save
                    this.DbContext.Companies.Add(dbCompany);
                }
                await this.DbContext.SaveChangesAsync();

                companyDetails.CompanyId = dbCompany.CompanyId;
                return companyDetails;
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
        }

        public async Task<Models.Company> GetCompanyById(int companyId)
        {
            var result = new Models.Company();
            try
            {
                if (companyId > 0)
                {
                    var dbCompany = await this.DbContext.Set<DataModel.Company>().FindAsync(companyId).ConfigureAwait(false);
                    result = Mapper.Map<Models.Company>(dbCompany);
                }
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        public async Task<List<Models.Company>> GetCompanys()
        {
            var result = new List<Models.Company>();
            try
            {

                var dbCompanys = await this.DbContext.Set<DataModel.Company>().ToListAsync().ConfigureAwait(false);
                result = Mapper.Map<List<Models.Company>>(dbCompanys);
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        Task<Models.Company> ICompanyService.GetCompanyById(int companyId)
        {
            throw new NotImplementedException();
        }

        Task<Models.Company> ICompanyService.GetCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        Task<List<Models.Company>> ICompanyService.GetCompanys()
        {
            throw new NotImplementedException();
        }

        Task<Models.Company> ICompanyService.SaveOrUpdate(Models.Company companyDetails)
        {
            throw new NotImplementedException();
        }
    }
}
