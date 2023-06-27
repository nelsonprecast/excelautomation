namespace ExcelAutomation.Models
{
    public class ProjectDetailDto
    {
        public int Index { get; set; }
        public int ProjectDetailId { get; set; }
        public string WD { get; set; }
        public string ItemName { get; set; }
        public string DispositionSpecialNote { get; set; }
        public string DetailPage { get; set; }
        public string TakeOffColor { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Pieces { get; set; }

        public string ImagePath { get; set; }

        public string? TotalLf { get; set; }

        public string? ActSfcflf { get; set; }

        public string? ActCfpcs { get; set; }

        public string? TotalActCf { get; set; }

        public string? NomCflf { get; set; }

        public string? NomCfpcs { get; set; }

        public string? TotalNomCf { get; set; }

        public string? MoldQty { get; set; }

        public string? LineItemCharge { get; set; }

        public string? TotalActualNominalValue { get; set; }

        public string PlanElevation { get; set; }

        public string Category { get; set; }

        public string LFValue { get; set; }

        public ICollection<PlanElevationReferenceDto> PlanElevationReferences { get; set; }

    }
}
