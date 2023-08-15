using Core.Domain;

namespace Service.Interfaces
{
    public interface IPlanElevationReferenceService
    {
        ICollection<PlanElevationReference> GetPlanElevationReferenceByProjectDetailId(int projectDetailId);

        ICollection<PlanElevationReference> GetPlanElevationReferenceByProjectDetailIds(int[] projectDetailIds);

        void CreatePlanElevationReference(PlanElevationReference planElevationReference);

        bool UpdatePlanElevationReference(PlanElevationReference planElevationReference);

    }
}
