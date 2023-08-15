namespace Core.Model.Response
{
    public class PlanElevationReferenceResponse : BaseModelEntity
    {
        public int ProjectDetailId { get; set; }

        public string PlanElevationValue { get; set; }

        public string LFValue { get; set; }

        public string ImagePath { get; set; }

        public string PageRefPath { get; set; }

        public string PcsValue { get; set; }

        public virtual ProjectDetailResponse ProjectDetail { get; set; } = null!;
    }
}
