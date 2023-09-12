using Core.Domain;
using Infrastructure.Data.Interfaces;
using Service.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace Service.Implementation
{
    public class ProjectService : BaseService<Project>, IProjectService
    {

        public ProjectService(IRepository<Project> repository) : base(repository)
        {
            
        }


        public ICollection<Project> GetProjects(string status)
        {
            var query = Repository.Table;
            if (status.Equals("Active"))
                query = query.Where(x => x.Status == null || x.Status.Equals(status));
            else if(!status.Equals("All"))
                query = query.Where(x => x.Status.Equals(status));
            return query.ToList();
        }

        public Project GetProjectById(int projectId)
        {
            return Repository.GetById(projectId);
        }

        public bool UpdateProject(Project project)
        {
            return Repository.Update(project);
        }

        public void SaveProject(Project project)
        {
            Repository.Insert(project);
        }

        public void ChangeProjectsStatus(ICollection<Project> projects)
        {
            Repository.Update(projects);
        }

        public ICollection<Project> GetProjectByIds(int[] ids)
        {
            return Repository.Table.Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
