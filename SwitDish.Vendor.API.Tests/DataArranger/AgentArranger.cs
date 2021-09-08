using SwitDish.DataModel_OLD.Models;
using System.Linq;

namespace SwitDish.Vendor.API.Tests.DataArranger
{
    public class AgentArranger : DataArranger<Agent>
    {
        private readonly SwitDishDatabaseContext dbContext;

        private readonly Agent agent;

        public AgentArranger(SwitDishDatabaseContext dbContext, Agent agent, Behaviour createFlag = Behaviour.TakeOwnership)
            : base(dbContext, agent, createFlag)
        {
            this.dbContext = dbContext;
            this.agent = agent;
        }

        public static Agent DefaultTestData()
        {
            return new Agent
            {
                CompanyId = 1,
                UserId = 1
            };
        }

        public static Agent CreateTestData(long companyId, long vendorId, long userId)
        {
            return new Agent
            {
                CompanyId = companyId,
                UserId = userId
            };
        }

        public bool ExistsByBusinessKey()
        {
            var agentEntity = this.dbContext.Agents.Where(d=>d.AgentId.Equals(this.agent))
                .FirstOrDefault();

            if (agentEntity == null)
            {
                return false;
            }

            return agentEntity.AgentId != 0;
        }
    }
}
