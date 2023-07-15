namespace ExcelAutomation.Models
{
    public class PlanElevationReferenceDto
    {
        public int PlanElevationReferanceId { get; set; }

        public int ProjectDetailId { get; set; }

        public string PlanElevationValue { get; set; }

        public string LFValue { get; set; }

        public string ImagePath { get; set; }

        public string PageRefPath { get; set; }

        public int? OriginalPlanElevationRefernceId { get; set; }
        public string PcsValue { get; set; }
    }
}
