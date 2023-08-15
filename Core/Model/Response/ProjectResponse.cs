namespace Core.Model.Response
{
    public class ProjectResponse : BaseModelEntity
    {
        public string ProjectName { get; set; }

        public string NominalCF { get; set; }

        public string ActualCF { get; set; }

        public string LineItemTotal { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? RevisionDate { get; set; }

        public string ContactSpecs { get; set; }
        public string Notes { get; set; }
        public string OpportunityId { get; set; }
        public string AccountName { get; set; }
        public ICollection<ProjectDetailResponse> ProjectDetails { get; set; }
        public ICollection<PlanElevationTextResponse> PlanElevationText { get; set; }
    }
}
