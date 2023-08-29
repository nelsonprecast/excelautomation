using Core.Domain;
using Microsoft.AspNetCore.Http;

namespace Facade.Interfaces
{
    public interface IPlanElevationReferenceFacade
    {
        void SavePlanElevationReference(ICollection<IFormFile> files, ICollection<IFormFile> ifiles, int projectDetailId, string planElevationReferenceJson);

        ICollection<PlanElevationReference> GetPlanElevationReferenceByProjectDetailId(int projectDetailId);

        void DeleteProjectPlanElevationReferance(int id);
    }
}
