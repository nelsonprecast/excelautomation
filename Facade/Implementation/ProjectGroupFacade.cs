using Facade.Interfaces;
using Service.Interfaces;

namespace Facade.Implementation
{
    public class ProjectGroupFacade : IProjectGroupFacade
    {
        private readonly IProjectGroupService _projectGroupService;

        public ProjectGroupFacade(IProjectGroupService projectGroupService)
        {
            _projectGroupService = projectGroupService;;
        }

        public bool EditGroup(int groupId, string groupName)
        {
            var projectGroup = _projectGroupService.GetProjectGroupById(groupId);
            projectGroup.GroupName = groupName;
            return _projectGroupService.EditGroup(projectGroup);
        }

        public bool DeleteGroup(int groupId)
        {
            var projectGroup = _projectGroupService.GetProjectGroupById(groupId);
            return _projectGroupService.DeleteGroup(projectGroup);
        }
    }
}
