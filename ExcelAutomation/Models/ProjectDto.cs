namespace ExcelAutomation.Models
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public ICollection<ProjectDetailDto> ProjectDetails { get; set; }
    }
}
