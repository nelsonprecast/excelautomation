using Core.Domain;

namespace Service.Interfaces
{
    public interface IProjectService
    {
        ICollection<Project> GetProjects(string status);

        Project GetProjectById(int projectId);

        bool UpdateProject(Project project);

        void SaveProject(Project project);
    }
}
