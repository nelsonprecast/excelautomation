using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IPlanElevationReferenceService
    {
        int SaveGroup(PlanElevationReferenceDto planElevationReferenceDto,int projectDetailId);
       
    }
}
