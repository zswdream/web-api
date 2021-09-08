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
    public class SuggestionController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public SuggestionController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("Suggestions/GetSuggestions")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSuggestions()
        {
            var list = new List<Suggestion>();
            list = _dbContext.Suggestions.ToList();
            List<SuggestionVM> ListS = new List<SuggestionVM>();
            try
            { 
                foreach (var item in list)
                {
                    SuggestionVM SVM = new SuggestionVM();
                    SVM.ByEmployee = item.ByEmployee;
                    SVM.CreateddDate = item.CreatedDate.ToString("M/d/yyyy");
                    SVM.SuggestionAbout = item.SuggestionAbout;
                    SVM.SuggestionBy = item.SuggestionBy;
                    SVM.SuggestionText = item.SuggestionText;
                    SVM.SuggestionId = item.SuggestionId;
                    ListS.Add(SVM);
                }

                return this.Ok(ListS);

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

        [Route("Suggestions/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdate(Suggestion suggestion)
        {
            try
            {
                var list = new List<Suggestion>();
                if (suggestion.SuggestionId > 0)
                {
                    suggestion.CreatedDate = DateTime.Now;
                    _dbContext.Entry(suggestion).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.Suggestions.ToList();
                    List<SuggestionVM> ListS = new List<SuggestionVM>();
                    try
                    {
                        foreach (var item in list)
                        {
                            SuggestionVM SVM = new SuggestionVM();
                            SVM.ByEmployee = item.ByEmployee;
                            SVM.CreateddDate = item.CreatedDate.ToString("M/d/yyyy");
                            SVM.SuggestionAbout = item.SuggestionAbout;
                            SVM.SuggestionBy = item.SuggestionBy;
                            SVM.SuggestionText = item.SuggestionText;
                            SVM.SuggestionId = item.SuggestionId;
                            ListS.Add(SVM);
                        }

                        return this.Ok(ListS);

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
                else
                {
                     suggestion.CreatedDate = DateTime.Now;
                    _dbContext.Suggestions.Add(suggestion);
                    _dbContext.SaveChanges();
                    list = _dbContext.Suggestions.ToList();
                    List<SuggestionVM> ListS = new List<SuggestionVM>();
                    try
                    {
                        foreach (var item in list)
                        {
                            SuggestionVM SVM = new SuggestionVM();
                            SVM.ByEmployee = item.ByEmployee;
                            SVM.CreateddDate = item.CreatedDate.ToString("M/d/yyyy");
                            SVM.SuggestionAbout = item.SuggestionAbout;
                            SVM.SuggestionBy = item.SuggestionBy;
                            SVM.SuggestionText = item.SuggestionText;
                            SVM.SuggestionId = item.SuggestionId;
                            ListS.Add(SVM);
                        }

                        return this.Ok(ListS);

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
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok(response);
            }
        }

        [Route("Suggestions/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(Suggestion suggestion)
        {
            try
            {
                var list = new List<Suggestion>();
                suggestion = _dbContext.Suggestions.Where(a => a.SuggestionId == suggestion.SuggestionId).FirstOrDefault();
                _dbContext.Suggestions.Remove(suggestion);
                _dbContext.SaveChanges();
                list = _dbContext.Suggestions.ToList();
                List<SuggestionVM> ListS = new List<SuggestionVM>();
                try
                {
                    foreach (var item in list)
                    {
                        SuggestionVM SVM = new SuggestionVM();
                        SVM.ByEmployee = item.ByEmployee;
                        SVM.CreateddDate = item.CreatedDate.ToString("M/d/yyyy");
                        SVM.SuggestionAbout = item.SuggestionAbout;
                        SVM.SuggestionBy = item.SuggestionBy;
                        SVM.SuggestionText = item.SuggestionText;
                        SVM.SuggestionId = item.SuggestionId;
                        ListS.Add(SVM);
                    }

                    return this.Ok(ListS);

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