﻿namespace Core.Domain
{
    public class PlanElevationText : BaseEntity
    {
        public string Text { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ImageSnipPath { get; set; }
        public string? PageRefImagePath { get; set; }
    }
}
