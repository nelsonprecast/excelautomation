namespace ExcelAutomation.Data
{
    public partial class PlanElevationReferance
    {
        public int PlanElevationReferanceId { get; set; }

        public int ProjectDetailId { get; set; }

        public string PlanElevationValue { get; set; }

        public string LFValue { get; set; }

        public string ImagePath { get; set; }

        public virtual ProjectDetail ProjectDetail { get; set; } = null!;
    }
}
