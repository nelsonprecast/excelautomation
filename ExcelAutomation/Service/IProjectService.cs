using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IProjectService
    {
        void SaveProject(ProjectDto project);

        ICollection<ProjectDto> GetProjects();

        ProjectDto GetProjectById(int  projectId);

        void UpdateProjectDetail(ProjectDto projectDto);
    }
}
