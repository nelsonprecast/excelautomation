using Core.Domain;
using Infrastructure.Data.Interfaces;
using Service.Interfaces;

namespace Service.Implementation
{
    public class ProjectDetailService : BaseService<ProjectDetail>, IProjectDetailService
    {
        public ProjectDetailService(IRepository<ProjectDetail> repository) : base(repository)
        {

        }

        public void CreateProjectDetail(ProjectDetail projectDetail)
        {
            Repository.Insert(projectDetail);
        }

        public void CreateProjectDetail(ICollection<ProjectDetail> entities)
        {
            Repository.Insert(entities);
        }

        public ICollection<ProjectDetail> GetProjectDetailByProjectId(int projectId)
        {
            return Repository.Table.Where(x => x.ProjectId == projectId).ToList();
        }

        public ICollection<ProjectDetail> GetProjectDetailByIds(int[] ids)
        {
            return Repository.Table.Where(x => ids.Contains(x.Id)).ToList();
        }

        public ProjectDetail GetProjectDetailById(int id)
        {
            return Repository.GetById(id);
        }

        public void DeleteProjectDetail(ProjectDetail projectDetail)
        {
            Repository.Delete(projectDetail);
        }

        public bool UpdateProjectDetail(ProjectDetail entity)
        {
            return Repository.Update(entity);
        }

        public bool UpdateProjectDetail(ICollection<ProjectDetail> entities)
        {
            return Repository.Update(entities);
        }
    }
}
