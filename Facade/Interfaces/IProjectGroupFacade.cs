using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Interfaces
{
    public interface IProjectGroupFacade
    {
        bool EditGroup(int groupId, string groupName);

        bool DeleteGroup(int groupId);
    }
}
