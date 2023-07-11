using Azure.Core;
using ExcelAutomation.Data;
using ExcelAutomation.Models;
using ExcelAutomation.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using Razor.Templating.Core;
using iText.Html2pdf;
using System.Drawing;
using System.Net.Http.Headers;

namespace ExcelAutomation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectService _projectService;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IRazorTemplateEngine _engine;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPlanElevationReferenceService _planElevationReferenceService;

        public HomeController(ILogger<HomeController> logger, IProjectService projectService, IWebHostEnvironment environment, IWebHostEnvironment webHostEnvironment, IPlanElevationReferenceService planElevationReferenceService)
        {
            _logger = logger;
            _projectService = projectService;
            _hostingEnvironment = environment;
            _webHostEnvironment = webHostEnvironment;
            _planElevationReferenceService = planElevationReferenceService;
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
            if (!string.IsNullOrEmpty(Request.Form["RevisionDate"]))
                project.RevisionDate = DateTime.ParseExact(Request.Form["RevisionDate"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
            if (Request.Form.Files["ContactSpecs"] != null)
            {
                var file = Request.Form.Files["ContactSpecs"];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ContactSpecs\\") + fileName;
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                project.ContactSpecs = "/ContactSpecs/" + fileName;
            }

            while (!string.IsNullOrEmpty(Request.Form[wd]))
            {
                var projectDetail = new ProjectDetailDto();
                projectDetail.WD = Request.Form[wd];
                projectDetail.ItemName = Request.Form["row" + i + "ItemName"];
                projectDetail.DispositionSpecialNote = Request.Form["row" + i + "DispositionSpecialNote"];
                projectDetail.DetailPage = Request.Form["row" + i + "DetailPage"];
                projectDetail.TakeOffColor = Request.Form["row" + i + "TakeOffColor"];
                projectDetail.Length = Request.Form["row" + i + "LengthHidden"];
                projectDetail.Width = Request.Form["row" + i + "WidthHidden"];
                projectDetail.Height = Request.Form["row" + i + "HeightHidden"];
                projectDetail.Pieces = Request.Form["row" + i + "Pieces"];
                projectDetail.TotalLf = Request.Form["row" + i + "TotalLF"];
                projectDetail.ActSfcflf = Request.Form["row" + i + "ActSFCFLF"];
                projectDetail.ActCfpcs = Request.Form["row" + i + "ActCFPcs"];
                projectDetail.TotalActCf = Request.Form["row" + i + "TotalActCF"];
                projectDetail.NomCflf = Request.Form["row" + i + "NomCFLF"];
                projectDetail.NomCfpcs = Request.Form["row" + i + "NomCFPcs"];
                projectDetail.TotalNomCf = Request.Form["row" + i + "TotalNomCF"];
                projectDetail.MoldQty = Request.Form["row" + i + "MoldQTY"];
                projectDetail.PlanElevation = Request.Form["row" + i + "PlanElevationHidden"];
                projectDetail.LFValue = Request.Form["row" + i + "TotalLFHidden"];
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

                projectDetail.PlanElevationFiles =
                    Request.Form.Files.Where(x => x.Name.Contains($"hiddenPlanElevationFile{i}_")).ToList();
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

        [HttpGet]
        public async  Task<IActionResult> ConvertToPdf(int projectId)
        {
            var project = _projectService.GetProjectById(projectId);
            var viewDataOrViewBag = new Dictionary<string, object>();
            viewDataOrViewBag["logo"] = ReturnBase64Image("images/np_logo.png");
            
            foreach (var projectDetail in project.ProjectDetails) {
                
                projectDetail.ImagePath = ReturnBase64Image(projectDetail.ImagePath);
                if (projectDetail.PlanElevationReferences != null)
                {
                    foreach (var planReference in projectDetail.PlanElevationReferences)
                    {
                        planReference.ImagePath = ReturnBase64Image(planReference.ImagePath);
                    }
                }
            }
            var projectHtml = await RazorTemplateEngine.RenderAsync("~/Views/Home/ProjectDetailPdf.cshtml", project,viewDataOrViewBag);
            using (MemoryStream stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(projectHtml, stream);
                return File(stream.ToArray(), "application/pdf", "ProjectDetail.pdf");
            }
        }
        [HttpPost]
        public  JsonResult UploadImages(ICollection<IFormFile> files,int projectDetailId,string pElevationJsonArray)
        {
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "PlanElevation");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                         file.CopyTo(fileStream);
                    }
                }
            }
            var pElevationList = DoDeserilization(pElevationJsonArray);
            foreach (var pElevation in pElevationList) { 
                if (pElevation != null && pElevation.PlanElevationReferanceId < 0)
                {
                    pElevation.OriginalPlanElevationRefernceId = pElevation.PlanElevationReferanceId;
                  pElevation.PlanElevationReferanceId=  _planElevationReferenceService.Save(pElevation,projectDetailId);  
                  pElevation.ImagePath = "/PlanElevation/" + pElevation.ImagePath;
                }
            }
            return new JsonResult(JsonConvert.SerializeObject(pElevationList));
        }

        private List<PlanElevationReferenceDto> DoDeserilization(string pElevationJsonArray)
        {
           return JsonConvert.DeserializeObject<List<PlanElevationReferenceDto>>(pElevationJsonArray);
        }

        private string ReturnBase64Image(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return "";
            }
            string webRootPath = _webHostEnvironment.WebRootPath;
 
            var fullPath = webRootPath + "/" + imagePath;
            if (!System.IO.File.Exists(fullPath))
                return string.Empty;
            byte[] imageArray = System.IO.File.ReadAllBytes(fullPath);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
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
            if (!string.IsNullOrEmpty(Request.Form["RevisionDate"]))
                project.RevisionDate = DateTime.ParseExact(Request.Form["RevisionDate"], "MM/dd/yyyy",
                    CultureInfo.InvariantCulture);
            if (Request.Form.Files["ContactSpecs"] != null)
            {
                var file = Request.Form.Files["ContactSpecs"];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ContactSpecs\\") + fileName;
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                project.ContactSpecs = "/ContactSpecs/" + fileName;
            }

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
                projectDetail.Length = Request.Form["row" + i + "LengthHidden"];
                projectDetail.Width = Request.Form["row" + i + "WidthHidden"];
                projectDetail.Height = Request.Form["row" + i + "HeightHidden"];
                projectDetail.Pieces = Request.Form["row" + i + "Pieces"];
                projectDetail.TotalLf = Request.Form["row" + i + "TotalLF"];
                projectDetail.ActSfcflf = Request.Form["row" + i + "ActSFCFLF"];
                projectDetail.ActCfpcs = Request.Form["row" + i + "ActCFPcs"];
                projectDetail.TotalActCf = Request.Form["row" + i + "TotalActCF"];
                projectDetail.NomCflf = Request.Form["row" + i + "NomCFLF"];
                projectDetail.NomCfpcs = Request.Form["row" + i + "NomCFPcs"];
                projectDetail.TotalNomCf = Request.Form["row" + i + "TotalNomCF"];
                projectDetail.MoldQty = Request.Form["row" + i + "MoldQTY"];
                projectDetail.PlanElevation = Request.Form["row" + i + "PlanElevationHidden"];
                projectDetail.PlanElevationJson = Request.Form["row" + i + "PlanElevationJsonHidden"];
                projectDetail.LFValue = Request.Form["row" + i + "TotalLFHidden"];
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
                if(!string.IsNullOrEmpty(projectDetail.PlanElevationJson)) {
                    projectDetail.PlanElevationReferences = JsonConvert.DeserializeObject<ICollection<PlanElevationReferenceDto>>(projectDetail.PlanElevationJson);
                }
                projectDetail.PlanElevationFiles =
                    Request.Form.Files.Where(x => x.Name.Contains($"hiddenPlanElevationFile{i}_")).ToList();
                projectDetails.Add(projectDetail);
                i++;
                wd = "row" + i + "WD";
            }
            project.ProjectDetails = projectDetails;
            _projectService.UpdateProjectDetail(project);
            return RedirectToAction("Edit",new{id= project.ProjectId });
        }

        public IActionResult GetProjectDetailView(int id)
        {
            var projectDetailDto = new ProjectDetailDto();
            projectDetailDto.Index = id;
            return PartialView("_ProjectDetailPartial", projectDetailDto);
        }

        [HttpPost]
        public IActionResult DeleteRow(int id)
        {
           var projectId=  _projectService.DeleteProjectDetailRow(id);
           return RedirectToAction("Edit", new {id = projectId});
        }

        [HttpPost]
        public IActionResult CreateGroup(ProjectGroupDto projectGroupDto)
        {
            var projectId = _projectService.SaveGroup(projectGroupDto);
            return RedirectToAction("Edit", new { id = projectId });
        }


        public IActionResult CopyProject(int id)
        {
            var newProjectId = _projectService.CopyProject(id);
            return RedirectToAction("Edit", new { id = newProjectId });
        }

        [HttpPost]
        public IActionResult DeleteProjectPlanElevation(int id)
        {
            _projectService.DeleteProjectPlanElevationReferances(id);
            return Ok();
        }
    }
}