using Core.Domain;
using Core.Model.Request;
using Facade.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Facade.Implementation
{
    public class ProjectGroupFacade : IProjectGroupFacade
    {
        private readonly IProjectGroupService _projectGroupService;
        private readonly IProjectDetailService _projectDetailService;

        public ProjectGroupFacade(IProjectGroupService projectGroupService,IProjectDetailService projectDetailService)
        {
            _projectGroupService = projectGroupService;
            _projectDetailService = projectDetailService;
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

        public bool RemoveFromGroup(string projectDetailIds)
        {
            var projectDetailIdArray = projectDetailIds.Split(',').Select(x => int.Parse(x));
            var projectDetails = _projectDetailService.GetProjectDetailByIds(projectDetailIdArray.ToArray());
            foreach (var projectDetail in projectDetails)
            {
                projectDetail.GroupId = null;
            }

            _projectDetailService.UpdateProjectDetail(projectDetails);
            return true;
        }

        public void ChangeGroup(int projectDetailId, int groupId)
        {
            var projectDetail = _projectDetailService.GetProjectDetailById(projectDetailId);
            projectDetail.GroupId = groupId < 0 ? null : groupId;
            _projectDetailService.UpdateProjectDetail(projectDetail);
        }

        public int SaveGroup(ProjectGroupRequest request)
        {
            var projectGroup = new ProjectGroup();
            projectGroup.GroupName = request.GroupName;
            projectGroup.CreatedDate = DateTime.Now;

            _projectGroupService.CreateGroup(projectGroup);

            var projectDetails = _projectDetailService.GetProjectDetailByIds(request.ProjectDetailIds.ToArray());

            foreach (var projectDetail in projectDetails)
            {
                projectDetail.GroupId = projectGroup.Id;
                _projectDetailService.UpdateProjectDetail(projectDetail);
            }

            return projectDetails.FirstOrDefault().ProjectId;
        }
    }
}
