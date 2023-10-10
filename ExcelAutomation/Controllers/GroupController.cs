using Core.Model.Request;
using Facade.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAutomation.Controllers
{
    public class GroupController : Controller
    {
        private readonly IProjectGroupFacade _projectGroupFacade;

        public GroupController(IProjectGroupFacade projectGroupFacade)
        {
            _projectGroupFacade = projectGroupFacade;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGroup(ProjectGroupRequest request)
        {
            var projectId = _projectGroupFacade.SaveGroup(request);
            return RedirectToAction("Edit", new { id = projectId });
        }

        [HttpGet]
        public IActionResult ChangeGroup(int projectDetailId, int GroupId)
        {
            if (GroupId == 0) return new BadRequestResult();
            _projectGroupFacade.ChangeGroup(projectDetailId, GroupId);
            return new JsonResult(true);
        }

        [HttpPost]
        public IActionResult UpdateGroup(int groupId, string groupName)
        {
            _projectGroupFacade.EditGroup(groupId, groupName);
            return new JsonResult(true);
        }

        [HttpPost]
        public IActionResult DeleteGroup(int groupId)
        {
            _projectGroupFacade.DeleteGroup(groupId);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult RemoveFromGroup(string projectDetailIds)
        {
            _projectGroupFacade.RemoveFromGroup(projectDetailIds);
            return new JsonResult(true);
        }
    }
}
