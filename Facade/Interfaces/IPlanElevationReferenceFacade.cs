using Microsoft.AspNetCore.Http;

namespace Facade.Interfaces
{
    public interface IPlanElevationReferenceFacade
    {
        void SavePlanElevationReference(ICollection<IFormFile> files, ICollection<IFormFile> ifiles, int projectDetailId, string planElevationReferenceJson);
    }
}
