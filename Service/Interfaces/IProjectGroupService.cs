using Core.Domain;

namespace Service.Interfaces
{
    public interface IProjectGroupService
    {
        ICollection<ProjectGroup> GetProjectGroupByIds(int[] ids);

        ProjectGroup GetProjectGroupById(int projectGroupId);

        bool EditGroup(ProjectGroup projectGroup);

        bool DeleteGroup(ProjectGroup projectGroup);
    }
}
