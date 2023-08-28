using Core.Domain;
using Facade.Interfaces;
using Service.Interfaces;

namespace Facade.Implementation
{
    public class PlanElevationTextFacade : IPlanElevationTextFacade
    {
        private readonly IPlanElevationTextService _planElevationTextService;


        public PlanElevationTextFacade(IPlanElevationTextService planElevationTextService)
        {
            _planElevationTextService = planElevationTextService;
        }

        public ICollection<PlanElevationText> GetPlanElevationTextByProjectId(int projectId)
        {
            return _planElevationTextService.GetPlanElevationTextByProjectId(projectId);
        }

        public void Save(PlanElevationText planElevationText)
        {
            _planElevationTextService.Save(planElevationText);
        }

        public void Save(ICollection<PlanElevationText> planElevationTextObjects)
        {
            _planElevationTextService.Save(planElevationTextObjects);
        }
    }
}
