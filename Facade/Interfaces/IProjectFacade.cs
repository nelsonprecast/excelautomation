using Core.Model.Response;

namespace Facade.Interfaces
{
    public interface IProjectFacade
    {
        ICollection<ProjectResponse> GetProjects(string status);

        ProjectResponse GetProjectById(int projectId);

        Task UpdateProject();
    }
}
