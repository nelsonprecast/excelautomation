namespace ExcelAutomation.Models
{
    public class HomeIndexViewModel
    {
        public string Status { get; set; }
        public ICollection<ProjectDto> Projects { get; set; }
    }
}
