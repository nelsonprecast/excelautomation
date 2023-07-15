namespace ExcelAutomation.Models;

public class ProjectPlanElevationTextDto
{
    public int ProjectId { get; set; }
    public ICollection<PlanElevationTextDto> PlanElevationText { get; set; }
}

public class PlanElevationTextDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }
}