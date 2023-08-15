using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Response;

namespace Facade.Interfaces
{
    public interface IProjectDetailFacade
    {
        void UpdateProjectDetail(ProjectDetailResponse projectDetailResponse);
    }
}
