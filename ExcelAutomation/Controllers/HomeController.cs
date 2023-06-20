using Azure.Core;
using ExcelAutomation.Models;
using ExcelAutomation.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExcelAutomation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectService _projectService;
        private IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger,IProjectService projectService, IWebHostEnvironment environment)
        {
            _logger = logger;
            _projectService = projectService;
            _hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            return View(_projectService.GetProjects());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save()
        {
            int i = 1;
            var wd = "row" + i + "WD";
            var projectDetails = new List<ProjectDetailDto>();

            var project = new ProjectDto();
            project.ProjectName = Request.Form["projectname"];
            project.NominalCF = Request.Form["NominalCF"];
            project.ActualCF = Request.Form["ActualCF"];

            while (!string.IsNullOrEmpty(Request.Form[wd]))
            {
                var projectDetail = new ProjectDetailDto();
                projectDetail.WD = Request.Form[wd];
                projectDetail.ItemName = Request.Form["row" + i + "ItemName"];
                projectDetail.DispositionSpecialNote = Request.Form["row" + i + "DispositionSpecialNote"];
                projectDetail.DetailPage = Request.Form["row" + i + "DetailPage"];
                projectDetail.TakeOffColor = Request.Form["row" + i + "TakeOffColor"];
                projectDetail.Length = Request.Form["row" + i + "Length"];
                projectDetail.Width = Request.Form["row" + i + "Width"];
                projectDetail.Height = Request.Form["row" + i + "Height"];
                projectDetail.Pieces = Request.Form["row" + i + "Pieces"];
                projectDetail.TotalLf = Request.Form["row" + i + "TotalLF"];
                projectDetail.ActSfcflf = Request.Form["row" + i + "ActSFCFLF"];
                projectDetail.ActCfpcs = Request.Form["row" + i + "ActCFPcs"];
                projectDetail.TotalActCf = Request.Form["row" + i + "TotalActCF"];
                projectDetail.NomCflf = Request.Form["row" + i + "NomCFLF"];
                projectDetail.NomCfpcs = Request.Form["row" + i + "NomCFPcs"];
                projectDetail.TotalNomCf = Request.Form["row" + i + "TotalNomCF"];
                projectDetail.MoldQty = Request.Form["row" + i + "MoldQTY"];
                projectDetail.PlanElevation = Request.Form["row" + i + "PlanElevation"];
                projectDetail.Category = Request.Form["row" + i + "Category"];
                if (Request.Form.Files["row" + i + "File"] != null)
                {
                    var file = Request.Form.Files["row" + i + "File"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ProjectImages\\") + fileName;
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    projectDetail.ImagePath = "/ProjectImages/" + fileName;
                }

                projectDetails.Add(projectDetail);
                i++;
                wd = "row" + i + "WD";
            }
            project.ProjectDetails = projectDetails;
            
            var projectId = await _projectService.SaveProject(project);
            return RedirectToAction("Edit", new { id = projectId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(_projectService.GetProjectById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            int i = 1;
            var wd = "row" + i + "WD";
            var projectDetails = new List<ProjectDetailDto>();

            var project = new ProjectDto();
            project.ProjectId = Convert.ToInt32(Request.Form["projectId"]);
            project.ProjectName = Request.Form["projectname"];
            project.NominalCF = Request.Form["NominalCF"];
            project.ActualCF = Request.Form["ActualCF"];
            project.LineItemTotal = Request.Form["LineItemTotal"];

            while (!string.IsNullOrEmpty(Request.Form[wd]))
            {
                var projectDetail = new ProjectDetailDto();
                if (!string.IsNullOrEmpty(Request.Form["row" + i + "projectDetailId"]))
                {
                    projectDetail.ProjectDetailId = Convert.ToInt32(Request.Form["row" + i + "projectDetailId"]);
                }
                projectDetail.WD = Request.Form[wd];
                projectDetail.ItemName = Request.Form["row" + i + "ItemName"];
                projectDetail.DispositionSpecialNote = Request.Form["row" + i + "DispositionSpecialNote"];
                projectDetail.DetailPage = Request.Form["row" + i + "DetailPage"];
                projectDetail.TakeOffColor = Request.Form["row" + i + "TakeOffColor"];
                projectDetail.Length = Request.Form["row" + i + "Length"];
                projectDetail.Width = Request.Form["row" + i + "Width"];
                projectDetail.Height = Request.Form["row" + i + "Height"];
                projectDetail.Pieces = Request.Form["row" + i + "Pieces"];
                projectDetail.TotalLf = Request.Form["row" + i + "TotalLF"];
                projectDetail.ActSfcflf = Request.Form["row" + i + "ActSFCFLF"];
                projectDetail.ActCfpcs = Request.Form["row" + i + "ActCFPcs"];
                projectDetail.TotalActCf = Request.Form["row" + i + "TotalActCF"];
                projectDetail.NomCflf = Request.Form["row" + i + "NomCFLF"];
                projectDetail.NomCfpcs = Request.Form["row" + i + "NomCFPcs"];
                projectDetail.TotalNomCf = Request.Form["row" + i + "TotalNomCF"];
                projectDetail.MoldQty = Request.Form["row" + i + "MoldQTY"];
                projectDetail.PlanElevation = Request.Form["row" + i + "PlanElevation"];
                projectDetail.Category = Request.Form["row" + i + "Category"];
                if (!string.IsNullOrEmpty(Request.Form["rowFP" + i + "LineItemCharge"]))
                    projectDetail.LineItemCharge = Request.Form["rowFP" + i + "LineItemCharge"];
                if (!string.IsNullOrEmpty(Request.Form["rowFP" + i + "TotalCheckBox"]))
                    projectDetail.TotalActualNominalValue = Request.Form["rowFP" + i + "TotalCheckBox"];

                if (Request.Form.Files["row" + i + "File"] != null)
                {
                    var file = Request.Form.Files["row" + i + "File"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ProjectImages\\") + fileName;
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    projectDetail.ImagePath = "/ProjectImages/" + fileName;
                }

                projectDetails.Add(projectDetail);
                i++;
                wd = "row" + i + "WD";
            }
            project.ProjectDetails = projectDetails;
            _projectService.UpdateProjectDetail(project);
            return RedirectToAction("Edit",new{id= project.ProjectId });
        }
    }
}