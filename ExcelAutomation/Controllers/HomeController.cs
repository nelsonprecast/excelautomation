using Core.Domain;
using Core.Model.Request;
using ExcelAutomation.Models;
using Facade.Interfaces;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Razor.Templating.Core;
using System.Diagnostics;
using System.Globalization;
using IPlanElevationTextService = ExcelAutomation.Service.IPlanElevationTextService;

namespace ExcelAutomation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectFacade _projectFacade;
        private readonly IPlanElevationTextFacade _planElevationTextFacade;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPlanElevationReferenceFacade _planElevationReferenceFacade;
        private readonly IPlanElevationTextService _planElevationTextService;
        private readonly ISugarCrmFacade _sugarCrmFacade;
        



        public HomeController(IProjectFacade projectFacade,
            IWebHostEnvironment environment,
            IWebHostEnvironment webHostEnvironment,
            ISugarCrmFacade sugarCrmFacade,  
            IPlanElevationTextFacade planElevationTextFacade, 
            IProjectGroupFacade projectGroupFacade, 
            IPlanElevationReferenceFacade planElevationReferenceFacade,
            IPlanElevationTextService planElevationTextService)
        {
            _projectFacade = projectFacade;
            _hostingEnvironment = environment;
            _webHostEnvironment = webHostEnvironment;
            _sugarCrmFacade = sugarCrmFacade;
            _planElevationTextFacade = planElevationTextFacade;
            _planElevationReferenceFacade = planElevationReferenceFacade;
            _planElevationTextService = planElevationTextService;
        }

        public IActionResult Index(string status)
        {
            var model = new HomeIndexViewModel();
            if (string.IsNullOrEmpty(status))
            {
                status = "Active";
            }
            model.Status = status;
            model.Projects = _projectFacade.GetProjects(status);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetJobNameList(string searchString)
        {
                        
           var returnVal = _sugarCrmFacade.SearchOppertunities(searchString);

            return Ok(returnVal?.records);
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
            var project = new Project();
            project.ProjectName = Request.Form["projectname"];
            project.NominalCf = Request.Form["NominalCF"];
            project.ActualCf = Request.Form["ActualCF"];
            if (!string.IsNullOrEmpty(Request.Form["RevisionDate"]))
                project.RevisionDate = DateTime.ParseExact(Request.Form["RevisionDate"],"MM/dd/yyyy",CultureInfo.InvariantCulture);
            if (Request.Form.Files["ContactSpecs"] != null)
            {
                var file = Request.Form.Files["ContactSpecs"];
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ContactSpecs\\") + fileName;
                await using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                project.ContactSpecs = "/ContactSpecs/" + fileName;
            }

            project.Notes = Request.Form["notes"];
            project.OpportunityId = Request.Form["opportunityId"];
            project.AccountName = Request.Form["accountName"];
            _projectFacade.SaveProject(project);
            
            return RedirectToAction("Edit", new { id = project.Id });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new HomeEditViewModel();
            model.ActiveTab = "1";
            model.Project = _projectFacade.GetProjectById(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult SavePlanElevationText(string planElevationTextRequests,ICollection<IFormFile> imageSnipFiles, ICollection<IFormFile> pageRefFiles)
        {
            var PlanElevatinTextRequests =
                JsonConvert.DeserializeObject<PlanElevationTextRequest[]>(planElevationTextRequests);
           _planElevationTextFacade.Save(PlanElevatinTextRequests, imageSnipFiles, pageRefFiles);


            return new JsonResult(true);
        }


        [HttpGet]
        public async  Task<IActionResult> ConvertToPdf(int projectId)
        {
            var project = _projectFacade.GetProjectById(projectId);
            var viewDataOrViewBag = new Dictionary<string, object>();
            viewDataOrViewBag["logo"] = ReturnBase64Image("images/np_logo.png");
            
            foreach (var projectDetail in project.ProjectDetails) {
                
                projectDetail.ImagePath = ReturnBase64Image(projectDetail.ImagePath);
                if (projectDetail.PlanElevationReferences != null)
                {
                    foreach (var planReference in projectDetail.PlanElevationReferences)
                    {
                        planReference.ImagePath = ReturnBase64Image(planReference.ImagePath);
                        planReference.PageRefPath= ReturnBase64Image(planReference.PageRefPath);

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
            TempData["TabNumber"] = Request.Form["TabNumber"];
            TempData.Keep("TabNumber");
            
            await _projectFacade.UpdateProject();

            var model = new HomeEditViewModel();
            model.ActiveTab = (string)Request.Form["TabNumber"];
            model.Project = _projectFacade.GetProjectById(int.Parse(Request.Form["projectId"]));
            return View(model);
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
           var projectId=  _projectFacade.DeleteProjectDetailRow(id);
           return new JsonResult(true);
        }

        [HttpPost]
        public IActionResult DeletePlanElevationText(int id)
        {
            _planElevationTextService.DeletePlanElevationText(id);
            return new JsonResult(true);
        }

       


        public IActionResult CopyProject(int id)
        {
            var newProjectId = _projectFacade.CopyProject(id);
            return RedirectToAction("Edit", new { id = newProjectId });
        }

        [HttpPost]
        public IActionResult DeleteProjectPlanElevation(int id)
        {
            _planElevationReferenceFacade.DeleteProjectPlanElevationReferance(id);
            return new JsonResult(true);
        }

        [HttpPost]
        public IActionResult Archive()
        {
            var projectIds = Request.Form["ArchiveActive"];
            var projectIdArray = new List<int>();
            foreach (var projectId in projectIds)
                projectIdArray.Add(Convert.ToInt32(projectId));
            _projectFacade.ChangeProjectsStatus(projectIdArray.ToArray(), "Archived");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Active()
        {
            var projectIds = Request.Form["ArchiveActive"];
            var projectIdArray = new List<int>();
            foreach (var projectId in projectIds)
                projectIdArray.Add(Convert.ToInt32(projectId));
            _projectFacade.ChangeProjectsStatus(projectIdArray.ToArray(), "Active");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetPdfProposal(PdfProposalRequest pdfProposalRequest)
        {
            var viewDataOrViewBag = new Dictionary<string, object>();
            viewDataOrViewBag["logo"] = ReturnBase64Image("images/np_logo.png");
           
            var project = _projectFacade.GetProjectById(pdfProposalRequest.ProjectId);
            var model = new PdfProposalModel();
            model.Project = project;
            model.PdfProposal = pdfProposalRequest;

            var projectHtml = await RazorTemplateEngine.RenderAsync("~/Views/Home/PdfProposal.cshtml", model, viewDataOrViewBag);
            using (MemoryStream stream = new MemoryStream())
            {
                
                HtmlConverter.ConvertToPdf(projectHtml, stream);
                return File(stream.ToArray(), "application/pdf", $"PdfProposal-{DateTime.Now.ToLongTimeString()}.pdf");
            }
        }


    }

}