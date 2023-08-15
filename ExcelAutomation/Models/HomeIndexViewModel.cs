using Core.Model.Response;

namespace ExcelAutomation.Models
{
    public class HomeIndexViewModel
    {
        public string Status { get; set; }
        public ICollection<ProjectResponse> Projects { get; set; }
    }
}
