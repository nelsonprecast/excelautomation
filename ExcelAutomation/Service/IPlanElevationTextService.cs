using ExcelAutomation.Models;

namespace ExcelAutomation.Service;

public interface IPlanElevationTextService
{
    int Save(ProjectPlanElevationTextDto projectPlanElevationTextDto);
    int Update(PlanElevationTextDto planElevationTextDto);
    int DeletePlanElevationText(int id);
}