using Core.Domain;
using Infrastructure.Data.Interfaces;
using Service.Interfaces;

namespace Service.Implementation
{
    public class PlanElevationReferenceService : BaseService<PlanElevationReference>, IPlanElevationReferenceService
    {
        public PlanElevationReferenceService(IRepository<PlanElevationReference> repository) : base(repository)
        {

        }

        public void CreatePlanElevationReference(PlanElevationReference planElevationReference)
        {
            Repository.Insert(planElevationReference);
        }

        public ICollection<PlanElevationReference> GetPlanElevationReferenceByProjectDetailId(int projectDetailId)
        {
            return Repository.Table.Where(x => x.ProjectDetailId == projectDetailId).ToList();
        }

        public ICollection<PlanElevationReference> GetPlanElevationReferenceByProjectDetailIds(int[] projectDetailIds)
        {
            return Repository.Table.Where(x => projectDetailIds.Contains(x.ProjectDetailId)).ToList();
        }

        public bool UpdatePlanElevationReference(PlanElevationReference planElevationReference)
        {
            return Repository.Update(planElevationReference);
        }

        public PlanElevationReference GetPlanElevationReferanceById(int id)
        {
            return Repository.GetById(id);
        }

        public void DeleteProjectPlanElevationReferance(PlanElevationReference planElevationReference)
        {
            Repository.Delete(planElevationReference);
        }
    }
}
