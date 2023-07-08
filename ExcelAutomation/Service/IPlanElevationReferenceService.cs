using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IPlanElevationReferenceService
    {
        int Save(PlanElevationReferenceDto planElevationReferenceDto,int projectDetailId);
       ICollection<PlanElevationReferenceDto> GetByProjectDetailId(int projectDetailID);
    }
}
