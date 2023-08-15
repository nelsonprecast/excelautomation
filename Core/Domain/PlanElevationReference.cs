namespace Core.Domain
{
    public class PlanElevationReference : BaseEntity
    {
        public int ProjectDetailId { get; set; }

        public string PlanElevationValue { get; set; }

        public string LFValue { get; set; }

        public string? ImagePath { get; set; }

        public string? PageRefPath { get; set; }
        public string? PcsValue { get; set; }


        public virtual ProjectDetail ProjectDetail { get; set; } = null!;
    }
}
