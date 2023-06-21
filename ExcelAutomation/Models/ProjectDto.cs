namespace ExcelAutomation.Models
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public string NominalCF { get; set; }

        public string ActualCF { get; set; }

        public string LineItemTotal { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? RevisionDate { get; set; }

        public ICollection<ProjectDetailDto> ProjectDetails { get; set; }
    }
}
