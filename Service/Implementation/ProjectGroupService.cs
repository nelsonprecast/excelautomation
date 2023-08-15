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
    }
}
