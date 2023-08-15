namespace Core.Model.Response
{
    public class ProjectPlanElevationTextResponse : BaseModelEntity
    {
        public int ProjectId { get; set; }
        public ICollection<PlanElevationTextResponse> PlanElevationText { get; set; }
    }
}
