﻿using Microsoft.AspNetCore.Http;

namespace Core.Model.Response
{
    public class ProjectDetailResponse : BaseModelEntity
    {
        public int Index { get; set; }
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

        public string PlanElevationJson { get; set; }
        public string GroupName { get; set; }
        public int? GroupId { get; set; }
        public float? HoursPerMold { get; set; }

        public ICollection<PlanElevationReferenceResponse>? PlanElevationReferences { get; set; }

        public ICollection<IFormFile> PlanElevationFiles { get; set; }
    }
}
