using AutoMapper;
using DataModel;
using SwitDish.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;

namespace SwitDish.Domain.Services
{
    public class AgentService : ServiceBase, IAgentService
    {
        public AgentService(SwitDishDatabaseEntities dbContext) : base(dbContext)
        {

        }

        public async Task<Models.Agent> SaveOrUpdate(Models.Agent agentDetails)
        {
            try
            {
                //agentId = await this.SaveOrUpdateAgent(agentDetails).ConfigureAwait(false);
                agentDetails = await this.SaveOrUpdateAgentDetails(agentDetails).ConfigureAwait(false);
                return agentDetails;
            }
            catch (Exception ex)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
        }

        public async Task<Models.Agent> GetAgent(int agentId)
        {
            var result = new Models.Agent();
            try
            {
                Company obj = new Company();
                obj.Name = "ETeksol";
                obj.Description = "ETeksol Description";
                DbContext.Companies.Add(obj);
                DbContext.SaveChanges();
                //            if (agentId > 0)
                //            {
                //                // Using LINQ to query data in DbSet
                //                //https://www.learnentityframeworkcore.com/dbset/querying-data
                //                var query = await this.DbContext.Agents
                //                    .Where (a => a.AgentId == agentId)
                //                    .Include(u => u.User)
                //                    .Include(c => c.Company)
                //                    .Select( x => new Models.Agent()
                //                    {
                //                        AgentId = x.AgentId,
                //                        AgentFirstName = x.User.FirstName,
                //                        AgentLastName = x.User.LastName,
                //                        UserName = x.User.Username,
                //                        Email = x.User.Email,
                //                        Password = x.User.Password,
                //                        Phone = x.User.Phone,
                //                        Gender = !string.IsNullOrEmpty(x.User.Gender)? x.User.Gender.Trim() : string.Empty,
                //                        Title = x.User.Title,
                //                        CompanyId = x.CompanyId.HasValue ? x.CompanyId.Value : 0,
                //                        DelieveryCount = "3",
                //                        //DateRegistered = DateTime.Now,
                //                        IsActive = x.User.Active.HasValue ? x.User.Active.Value : false,
                //                        UserId = x.UserId
                //                    })
                //                    .SingleAsync().ConfigureAwait(false);

                //                result = query;
                //    // Using LINQ to SQL
                //    //var query = (from a in this.DbContext.Agents.c
                //    //            join u in this.DbContext.Users on a.AgentId equals u.UserId
                //    //            join v in this.DbContext.Vendors on a.AgentId equals v.VendorId into vendorGroup
                //    //            from vg in vendorGroup
                //    //            join c in this.DbContext.Companies on a.CompanyId equals c.CompanyId into companyGroup
                //    //            from cg in companyGroup
                //    //            select new {a})
                //}
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        public async Task<bool> DeleteAgentById(int agentId)
        {
            var result = false;
            try
            {
                if (agentId > 0)
                {
                    var dbAgent = await this.DbContext.Set<Agent>().FindAsync(agentId).ConfigureAwait(false);
                    var removedAgent = this.DbContext.Set<Agent>().Remove(dbAgent);
                    await this.DbContext.SaveChangesAsync();
                    result = true;
                }
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        public async Task<List<Models.Agent>> GetAgents()
        {
            var result = new List<Models.Agent>();
            try
            {

                var dbAgents = await this.DbContext.Set<Agent>().ToListAsync().ConfigureAwait(false);
                result = Mapper.Map<List<Models.Agent>>(dbAgents);
            }
            catch (Exception ex)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        private async Task<long> SaveOrUpdateAgent (Models.Agent agentDetails)
        {
            //save agent into user table
            var user = new DataModel.User()
            {
                UserId =agentDetails.UserId,
                FirstName = agentDetails.AgentFirstName,
                LastName = agentDetails.AgentLastName,
                Username = agentDetails.UserName,
                Password = agentDetails.Password,
                Email = agentDetails.Email,
                Phone = agentDetails.Phone,
                Gender = agentDetails.Gender,
                Title = agentDetails.Gender,

            };
            int userid =Convert.ToInt32(user.UserId);
            if (userid == 0)
            {
                this.DbContext.Users.Add(user);
            }
            else
            {
                this.DbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            }
            

        // If agent has its own company, save into company table
            var company = new DataModel.Company()
            {
                CompanyId = agentDetails.CompanyId,
                Name = "Test company",
                Description = "Test "
            };
            if (company.CompanyId == 0)
            {
                this.DbContext.Companies.Add(company);
            }
            else
            {
                this.DbContext.Entry(company).State = System.Data.Entity.EntityState.Modified;

            }
          

            // link agent to specific vendor
            var vendor = new DataModel.Vendor()
            {
                Name = agentDetails.VendorName
            };
            var VendorId = Convert.ToInt32(vendor.VendorId);
            if (VendorId == 0)
            {
                this.DbContext.Vendors.Add(vendor);
            }
            else
            {
                this.DbContext.Entry(vendor).State = System.Data.Entity.EntityState.Modified;
            }


            // save into agent table and rerefence all foreign keys
            var agent = new DataModel.Agent()
            {
                AgentId = agentDetails.AgentId,
                CompanyId = company.CompanyId,
                UserId = user.UserId
            };
            if (agent.AgentId == 0)
            {
                this.DbContext.Agents.Add(agent);
            }
            else
            {
                this.DbContext.Entry(agent).State = System.Data.Entity.EntityState.Modified;
            }
            
            await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
            return agent.AgentId;
        }

        private async Task<Models.Agent> SaveOrUpdateAgentDetails(Models.Agent agentDetails)
        {
            using (var transaction = this.DbContext.Database.BeginTransaction()){
                try
                {
                    //save agent into user table
                    long userId = await this.SaveOrUpdateUser(agentDetails).ConfigureAwait(false);

                    // If agent has its own company, save into company table
                    long companyId = await this.SaveOrUpdateCompany(agentDetails).ConfigureAwait(false);

                    // link agent to specific vendor
                    int vendorId = await this.SaveOrUpdateVendor(agentDetails).ConfigureAwait(false);

                    // save into agent table and rerefence all foreign keys
                    long agentId = await this.SaveOrUpdateAgent(agentDetails, vendorId, companyId, userId).ConfigureAwait(false);

                    await this.DbContext.SaveChangesAsync().ConfigureAwait(false);

                    agentDetails.AgentId = agentId;
                    agentDetails.CompanyId = companyId;
                    agentDetails.UserId = userId;
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                }
            }
            return agentDetails;
        }

        private async Task<long> SaveOrUpdateCompany(Models.Agent agentDetails)
        {
            long result = 0;
            if (agentDetails.CompanyId == 0)
            {
                var company = new DataModel.Company()
                {
                    Name = agentDetails.AgentCompanyName,
                    Description = agentDetails.AgentCompanyDescription
                };
                this.DbContext.Companies.Add(company);
                await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
                result = company.CompanyId;
            }
            else
            {
                var existingCompany = await this.DbContext.Companies.FindAsync(agentDetails.CompanyId).ConfigureAwait(false);
                existingCompany.Name = agentDetails.AgentCompanyName;
                existingCompany.Description = agentDetails.AgentCompanyDescription;
                this.DbContext.Entry(existingCompany).State = System.Data.Entity.EntityState.Modified;
                result = agentDetails.CompanyId;
            }
            await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        private async Task<long> SaveOrUpdateUser(Models.Agent agentDetails)
        {
            long result = 0;
            if (agentDetails.UserId == 0)
            {
                var user = new DataModel.User()
                {
                    UserId =agentDetails.UserId,
                    FirstName = agentDetails.AgentFirstName,
                    LastName = agentDetails.AgentLastName,
                    Username = agentDetails.UserName,
                    Password = agentDetails.Password,
                    Email = agentDetails.Email,
                    Phone = agentDetails.Phone,
                    Gender = agentDetails.Gender,
                    Title = agentDetails.Gender,
                };
                this.DbContext.Users.Add(user);
                await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
                result = Convert.ToUInt32(user.UserId);
            }
            else
            {
                var existingUser = await this.DbContext.Users.FindAsync(agentDetails.UserId).ConfigureAwait(false);
                existingUser.FirstName = agentDetails.AgentFirstName;
                existingUser.LastName = agentDetails.AgentLastName;
                existingUser.Username = agentDetails.UserName;
                existingUser.Password = agentDetails.Password;
                existingUser.Email = agentDetails.Email;
                existingUser.Phone = agentDetails.Phone;
                existingUser.Gender = agentDetails.Gender;
                existingUser.Title = agentDetails.Gender;
                this.DbContext.Entry(existingUser).State = System.Data.Entity.EntityState.Modified;
                result = agentDetails.UserId;
            }
            await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        private async Task<int> SaveOrUpdateVendor(Models.Agent agentDetails)
        {
            int result = 0;
            //if (agentDetails. == 0)
            //{
            //    var vendor = new DataModel.Vendor()
            //    {
            //        Name = agentDetails.VendorName,
            //        DateIncorporated = agentDetails.DateRegistered
            //    };
            //    this.DbContext.Vendors.Add(vendor);
            //    await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
            //    result = (int)vendor.VendorId;
            //}
            //else
            //{
            //    var existingVendor = await this.DbContext.Vendors.FindAsync(agentDetails.VendorId).ConfigureAwait(false);
            //    existingVendor.Name = agentDetails.VendorName;
            //    existingVendor.DateIncorporated = agentDetails.DateRegistered;
            //    this.DbContext.Entry(existingVendor).State = System.Data.Entity.EntityState.Modified;
            //}
            //await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        private async Task<long> SaveOrUpdateAgent(Models.Agent agentDetails, long vendorId, long companyId, long userId)
        {
            long result = 0;
            if (agentDetails.AgentId == 0)
            {
                var agent = new DataModel.Agent()
                {
                    CompanyId = companyId,
                    UserId = userId
                };
                this.DbContext.Agents.Add(agent);
                await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
                result = agent.AgentId;
            }
            else
            {
                var existingAgent = await this.DbContext.Agents.FindAsync(agentDetails.AgentId).ConfigureAwait(false);
                existingAgent.CompanyId = companyId;
                existingAgent.UserId =userId;
                this.DbContext.Entry(existingAgent).State = System.Data.Entity.EntityState.Modified;
                result = agentDetails.AgentId;
            }
            await this.DbContext.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

    }
}
