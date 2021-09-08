using SwitDish.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Contract
{
    public interface ICompanyService
    {
        Task<Company> GetCompanyById(int companyId);
        Task<Company> GetCompany(int companyId);
        Task<List<Company>> GetCompanys();
        Task<Models.Company> SaveOrUpdate(Company companyDetails);

    }
}
