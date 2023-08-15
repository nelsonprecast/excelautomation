using Core.Model.Response;
using Facade.Interfaces;
using Service.Interfaces;

namespace Facade.Implementation
{
    public class ProjectDetailFacade : IProjectDetailFacade
    {
        private readonly IProjectDetailService _projectDetailService;

        public ProjectDetailFacade(IProjectDetailService projectDetailService)
        {
            _projectDetailService = projectDetailService;
        }

        public void UpdateProjectDetail(ProjectDetailResponse projectDetailResponse)
        {

        }
    }
}
