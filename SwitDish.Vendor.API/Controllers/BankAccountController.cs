using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwitDish.DataModel_OLD.Models;
using SwitDish.Vendor.API.Models;

namespace SwitDish.Vendor.API.Controllers
{
    public class BankAccountController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public BankAccountController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("BankAccount/GetBankAccount")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBankAccount()
        {
            var list = new List<BankAccount>();
            try
            {
                list = _dbContext.BankAccounts.ToList();
                List<BankAccountVM> listVM = new List<BankAccountVM>();
                foreach (var item in list)
                {
                    BankAccountVM obj = new BankAccountVM();
                    obj.AccountId = Convert.ToUInt32(item.AccountId);
                    obj.BankName = item.BankName;
                    obj.AccountName = item.AccountName;
                    obj.SortCode = item.SortCode;
                    obj.AccountNumber = item.AccountNumber;
                    listVM.Add(obj);
                }
                return this.Ok(listVM);
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok(response);
            }
        }

        [Route("BankAccount/GetBankAccountByUserID")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBankAccountByUserID(long UesrID)
        {
            var list = new List<BankAccount>();
            try
            {
                list = _dbContext.BankAccounts.Where(x=>x.UserId == UesrID).ToList();
                List<BankAccountGetById> listVM = new List<BankAccountGetById>();
                foreach (var item in list)
                {
                    BankAccountGetById obj = new BankAccountGetById();
                    obj.accountId = Convert.ToUInt32(item.AccountId);
                    obj.bankName = item.BankName;
                    obj.accountName = item.AccountName;
                    obj.sortCode = item.SortCode;
                    obj.accountNumber = item.AccountNumber;
                    listVM.Add(obj);
                }
                return this.Ok(listVM);
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok(response);
            }
        }

    }
}