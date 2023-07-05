namespace ExcelAutomation.Models
{
    public class ProjectGroupDto
    {
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public List<int> ProjectDetailIds { get; set; }
    }
}
