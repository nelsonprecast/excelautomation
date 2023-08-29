using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IProjectService
    {
        int SaveGroup(ProjectGroupDto projectGroupDto);
        
        int DeleteProjectDetailRow(int id);
        
        void ChangeGroup(int projectDetailId, int groupId);
        
        bool RemoveFromGroup(string projectDetailIds);
    }
}
