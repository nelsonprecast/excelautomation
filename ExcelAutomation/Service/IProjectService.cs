using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IProjectService
    {
        int SaveGroup(ProjectGroupDto projectGroupDto);
        
        
    }
}
