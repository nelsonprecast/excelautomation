using Core.Domain;

namespace Service.Interfaces
{
    public interface IProjectDetailService
    {
        void CreateProjectDetail(ProjectDetail  projectDetail);

        void CreateProjectDetail(ICollection<ProjectDetail> entities);

        ICollection<ProjectDetail> GetProjectDetailByProjectId(int projectId);

        ICollection<ProjectDetail> GetProjectDetailByIds(int[] ids);

        ProjectDetail GetProjectDetailById(int id);

        void DeleteProjectDetail(ProjectDetail projectDetail);

        bool UpdateProjectDetail(ProjectDetail entity);

        bool UpdateProjectDetail(ICollection<ProjectDetail> entities);
        
    }
}
