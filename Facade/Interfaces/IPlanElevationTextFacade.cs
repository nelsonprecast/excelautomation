using Core.Domain;
using Core.Model.Request;
using Microsoft.AspNetCore.Http;

namespace Facade.Interfaces
{
    public interface IPlanElevationTextFacade
    {
        ICollection<PlanElevationText> GetPlanElevationTextByProjectId(int projectId);

        void Save(PlanElevationText planElevationText);

        void Save(ICollection<PlanElevationTextRequest> planElevationTextObjects, ICollection<IFormFile> imageSnipFiles, ICollection<IFormFile> pageRefFiles);
    }
}
