using Core.Domain;
using Infrastructure.Data.Interfaces;
using Service.Interfaces;

namespace Service.Implementation
{
    public class ProjectGroupService : BaseService<ProjectGroup>, IProjectGroupService
    {
        public ProjectGroupService(IRepository<ProjectGroup> repository) : base(repository)
        {

        }

        public ICollection<ProjectGroup> GetProjectGroupByIds(int[] ids)
        {
            return Repository.Table.Where(x => ids.Contains(x.Id)).ToList();
        }

        public ProjectGroup GetProjectGroupById(int projectGroupId)
        {
            return Repository.Table.FirstOrDefault(x => x.Id == projectGroupId);
        }

        public bool EditGroup(ProjectGroup projectGroup)
        {
            return Repository.Update(projectGroup);
        }

        public bool DeleteGroup(ProjectGroup projectGroup)
        {
            Repository.Delete(projectGroup);
            return true;
        }
    }
}
