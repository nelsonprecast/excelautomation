﻿using ExcelAutomation.Models;
using ExcelAutomation.Service;
using Facade.Interfaces;
using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Razor.Templating.Core;
using System.Diagnostics;
using System.Globalization;

namespace ExcelAutomation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectFacade _projectFacade;
        private readonly IProjectService _projectService;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPlanElevationReferenceService _planElevationReferenceService;
        private readonly IPlanElevationTextService _planElevationTextService;
        private readonly IConfiguration configuration;
        private readonly ISugarCrmFacade _sugarCrmFacade;

        

        public HomeController(IProjectFacade projectFacade,
            IWebHostEnvironment environment,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            ISugarCrmFacade sugarCrmFacade)
        {
            _projectFacade = projectFacade;
            _hostingEnvironment = environment;
            _webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
            _sugarCrmFacade = sugarCrmFacade;
        }

        public IActionResult Index(string status)
        {
            var model = new HomeIndexViewModel();
            if (string.IsNullOrEmpty(status))
            {
                status = "Active";
            }
            var opp = _sugarCrmFacade.SearchOppertunities("test");
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
            var project = new ProjectDto();
            project.ProjectName = Request.Form["projectname"];
            project.NominalCF = Request.Form["NominalCF"];
            project.ActualCF = Request.Form["ActualCF"];
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
            var projectId = await _projectService.SaveProject(project);

            var projectPlanElevationDto = new ProjectPlanElevationTextDto();
            projectPlanElevationDto.ProjectId = projectId;
            projectPlanElevationDto.PlanElevationText = new List<PlanElevationTextDto>();
            int i = 1;
            var key = "planElevationTextRow"+i;
            while (!string.IsNullOrEmpty(Request.Form[key]))
            {
                var dtoObject = new PlanElevationTextDto
                {
                    Text = Request.Form[key],
                    CreatedDate = DateTime.Now
                };
                projectPlanElevationDto.PlanElevationText.Add(dtoObject);
                i++;
                key = "planElevationTextRow" + i;
            }

            _planElevationTextService.Save(projectPlanElevationDto);

            return RedirectToAction("Edit", new { id = projectId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new HomeEditViewModel();
            model.ActiveTab = "1";
            model.Project = _projectFacade.GetProjectById(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult SavePlanElevationText(string planTextList,int projectId,string editPlanTextList)
        {
            var planTexts = JsonConvert.DeserializeObject<List<string>>(planTextList);
            var EditPlanTexts = JsonConvert.DeserializeObject<List<PlanElevationTextDto>>(editPlanTextList);
           // var projectDtoObject = JsonConvert.DeserializeObject<ProjectDto>(projectDto);

            if (planTexts != null)
                foreach (var planText in planTexts)
                {
                    _planElevationTextService.Save(new ProjectPlanElevationTextDto()
                    {
                        ProjectId = projectId,
                        PlanElevationText = new List<PlanElevationTextDto>()
                        {
                            new PlanElevationTextDto()
                            {
                                CreatedDate = DateTime.Now,
                                Text = planText
                            }
                        }
                    });
                }

            if (EditPlanTexts != null)
            {
                foreach (var objectDto in EditPlanTexts)
                {
                    _planElevationTextService.Update(new PlanElevationTextDto()
                    {
                        Id = objectDto.Id, CreatedDate = DateTime.Now, Text = objectDto.Text
                    });
                }
            }

           
            return new OkResult();
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
        [HttpPost]
        public  JsonResult UploadImages(ICollection<IFormFile> files, ICollection<IFormFile> ifiles, int projectDetailId,string pElevationJsonArray)
        {
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "PlanElevation");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fName = file.FileName.Substring(15);
                    using var fileStream = new FileStream(Path.Combine(uploads, fName), FileMode.Create);
                    file.CopyTo(fileStream);
                }
            }
            foreach (var file in ifiles)
            {
                if (file.Length > 0)
                {
                    var fName = file.FileName.Substring(15);
                    using var fileStream = new FileStream(Path.Combine(uploads, fName), FileMode.Create);
                    file.CopyTo(fileStream);
                }
            }
            var pElevationList = DoDeserilization(pElevationJsonArray);
            foreach (var pElevation in pElevationList) {
                if (pElevation.PlanElevationReferanceId < 0)
                {
                    pElevation.OriginalPlanElevationRefernceId = pElevation.PlanElevationReferanceId;
                    pElevation.PlanElevationReferanceId = _planElevationReferenceService.Save(pElevation,
                        projectDetailId);
                    pElevation.ImagePath = pElevation.ImagePath;
                    pElevation.PageRefPath = pElevation.PageRefPath;
                }
                else
                {
                    pElevation.PlanElevationReferanceId = _planElevationReferenceService.Update(pElevation, projectDetailId);
                }

            }
            return new JsonResult(JsonConvert.SerializeObject(_planElevationReferenceService.GetByProjectDetailId(projectDetailId)));
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
           var projectId=  _projectService.DeleteProjectDetailRow(id);
           return RedirectToAction("Edit", new {id = projectId});
        }

        [HttpPost]
        public IActionResult DeletePlanElevationText(int id)
        {
            _planElevationTextService.DeletePlanElevationText(id);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult CreateGroup(ProjectGroupDto projectGroupDto)
        {
            var projectId = _projectService.SaveGroup(projectGroupDto);
            return RedirectToAction("Edit", new { id = projectId });
        }

        [HttpGet]
        public IActionResult ChangeGroup(int projectDetailId,int GroupId)
        {
            if (GroupId == 0) return new BadRequestResult();
            _projectService.ChangeGroup(projectDetailId, GroupId);
            return new OkResult();
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

        [HttpPost]
        public IActionResult Archive()
        {
            var projectIds = Request.Form["ArchiveActive"];
            var projectIdArray = new List<int>();
            foreach (var projectId in projectIds)
                projectIdArray.Add(Convert.ToInt32(projectId));
            _projectService.ChangeProjectsStatus(projectIdArray.ToArray(), "Archived");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Active()
        {
            var projectIds = Request.Form["ArchiveActive"];
            var projectIdArray = new List<int>();
            foreach (var projectId in projectIds)
                projectIdArray.Add(Convert.ToInt32(projectId));
            _projectService.ChangeProjectsStatus(projectIdArray.ToArray(), "Active");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateGroup(int groupId, string groupName)
        {
            _projectService.EditGroup(groupId, groupName);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult DeleteGroup(int groupId)
        {
            _projectService.DeleteGroup(groupId);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult RemoveFromGroup(string projectDetailIds)
        {
            _projectService.RemoveFromGroup(projectDetailIds);
            return new OkResult();
        }
    }

}