using System.ComponentModel.DataAnnotations;

namespace ExcelAutomation.Data
{
    public class ProjectGroup
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ProjectDetail ProjectDetail { get; set; } = null!;
    }
}
