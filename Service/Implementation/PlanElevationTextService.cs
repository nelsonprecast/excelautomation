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

        public ICollection<PlanElevationText> getPlanElevationTextByProjectId(int projectId)
        {
            return Repository.Table.Where(x => x.ProjectId == projectId).ToList();
        }
    }
}
