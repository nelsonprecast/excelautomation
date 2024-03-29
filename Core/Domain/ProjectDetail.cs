﻿namespace Core.Domain
{
    public class ProjectDetail : BaseEntity
    {
        public int ProjectId { get; set; }

        public string? Wd { get; set; }

        public string ItemName { get; set; } = null!;

        public string DispositionSpecialNote { get; set; } = null!;

        public string? DetailPage { get; set; }

        public string? TakeOffColor { get; set; }

        public string? Length { get; set; }

        public string? Width { get; set; }

        public string? Height { get; set; }

        public string? Pieces { get; set; }

        public string? ImagePath { get; set; }

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

        public string? Category { get; set; }
        
        public int? GroupId { get; set; }

        public float? HoursPerMold { get; set; }
    }
}
