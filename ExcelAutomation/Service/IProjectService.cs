using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IProjectService
    {
        int SaveGroup(ProjectGroupDto projectGroupDto);
        Task<int> SaveProject(ProjectDto project);

        ICollection<ProjectDto> GetProjects(string status);

        ProjectDto GetProjectById(int  projectId);
        void UpdateProject(ProjectDto projectDto);
        void UpdateProjectDetail(ProjectDto projectDto);

        int DeleteProjectDetailRow(int id);

        int CopyProject(int id);

        void DeleteProjectPlanElevationReferances(int id);
        void ChangeGroup(int projectDetailId, int groupId);

        void ChangeProjectsStatus(int[] projectIds, string status);

        bool EditGroup(int groupId, string groupName);

        bool DeleteGroup(int groupId);

        bool RemoveFromGroup(string projectDetailIds);
    }
}
