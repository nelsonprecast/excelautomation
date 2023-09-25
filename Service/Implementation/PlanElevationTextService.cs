using Core.Domain;
using Infrastructure.Data.Interfaces;
using Service.Interfaces;

namespace Service.Implementation
{
    public class PlanElevationTextService : BaseService<PlanElevationText>, IPlanElevationTextService
    {
        public PlanElevationTextService(IRepository<PlanElevationText> repository) : base(repository)
        {

        }

        public ICollection<PlanElevationText> GetPlanElevationTextByProjectId(int projectId)
        {
            return Repository.Table.Where(x => x.ProjectId == projectId).ToList();
        }

        public void Save(PlanElevationText planElevationText)
        {
            Repository.Insert(planElevationText);
        }

        public void Save(ICollection<PlanElevationText> planElevationTextObjects)
        {
            Repository.Insert(planElevationTextObjects);
        }

        public void Update(ICollection<PlanElevationText> planElevationTextObjects)
        {
            Repository.Update(planElevationTextObjects);
        }

        public void Delete(PlanElevationText planElevationText) {  Repository.Delete(planElevationText); }

        public void Delete(ICollection<PlanElevationText> entities) {  Repository.Delete(entities);  }
    }
}
