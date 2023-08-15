using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Service.Interfaces
{
    public interface IPlanElevationTextService
    {
        ICollection<PlanElevationText> getPlanElevationTextByProjectId(int projectId);
    }
}
