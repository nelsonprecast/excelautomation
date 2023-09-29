namespace Core.Domain
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; set; } = null!;

        public string NominalCf { get; set; }

        public string ActualCf { get; set; }

        public string LineItemTotal { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? RevisionDate { get; set; }

        public string ContactSpecs { get; set; }
        public string Notes { get; set; }
        public string OpportunityId { get; set; }
        public string AccountName { get; set; }

        public string Status { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; } = new List<ProjectDetail>();
    }
}
