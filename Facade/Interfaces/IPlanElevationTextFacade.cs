using Core.Domain;

namespace Facade.Interfaces
{
    public interface IPlanElevationTextFacade
    {
        ICollection<PlanElevationText> GetPlanElevationTextByProjectId(int projectId);

        void Save(PlanElevationText planElevationText);

        void Save(ICollection<PlanElevationText> planElevationTextObjects);
    }
}
