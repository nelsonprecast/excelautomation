using Core.Domain;

namespace Service.Interfaces
{
    public interface IPlanElevationTextService
    {
        ICollection<PlanElevationText> GetPlanElevationTextByProjectId(int projectId);

        void Save(PlanElevationText planElevationText);

        void Save(ICollection<PlanElevationText> planElevationTextObjects);

        void Update(ICollection<PlanElevationText> planElevationTextObjects);

        void Delete(PlanElevationText planElevationText);

        void Delete(ICollection<PlanElevationText> entities);

        PlanElevationText GetPlanElevationTextById(int id);
    }
}
