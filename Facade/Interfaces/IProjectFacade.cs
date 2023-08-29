using Core.Domain;
using Core.Model.Response;

namespace Facade.Interfaces
{
    public interface IProjectFacade
    {
        ICollection<ProjectResponse> GetProjects(string status);

        ProjectResponse GetProjectById(int projectId);

        Task UpdateProject();

        void SaveProject(Project project);

        void ChangeProjectsStatus(int[] projectIds, string status);

        int CopyProject(int id);

        int DeleteProjectDetailRow(int id);
    }
}
