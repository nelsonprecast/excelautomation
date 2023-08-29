using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IProjectService
    {
        int SaveGroup(ProjectGroupDto projectGroupDto);
        
        void ChangeGroup(int projectDetailId, int groupId);
        
    }
}
