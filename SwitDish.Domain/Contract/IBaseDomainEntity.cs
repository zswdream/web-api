using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Contract
{
    public interface IBaseDomainEntity
    {
        int Id
        {
            get;
            set;
        }
    }
}
