using SwitDish.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Contract
{
    public interface IAgentService
    {
        Task<Agent> GetAgent(int agentId);
        Task<List<Agent>> GetAgents();
        Task<bool> DeleteAgentById(int agentId);
        Task<Models.Agent> SaveOrUpdate(Agent agentDetails);

    }
}
