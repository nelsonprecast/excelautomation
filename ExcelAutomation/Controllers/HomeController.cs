using ExcelAutomation.Models;
using ExcelAutomation.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Razor.Templating.Core;
using iText.Html2pdf;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ExcelAutomation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectService _projectService;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPlanElevationReferenceService _planElevationReferenceService;
        private readonly IPlanElevationTextService _planElevationTextService;
        private readonly IConfiguration configuration;
        

        public HomeController(IProjectService projectService,
            IWebHostEnvironment environment,
            IWebHostEnvironment webHostEnvironment,
            IPlanElevationReferenceService planElevationReferenceService,
            IPlanElevationTextService planTextService,
            IConfiguration configuration)
        {
            _projectService = projectService;
            _hostingEnvironment = environment;
            _webHostEnvironment = webHostEnvironment;
            _planElevationReferenceService = planElevationReferenceService;
            _planElevationTextService = planTextService;
            this.configuration = configuration;
        }

        public IActionResult Index(string status)
        {
            var model = new HomeIndexViewModel();
            if (string.IsNullOrEmpty(status))
            {
                status = "Active";
            }

            model.Status = status;
            model.Projects = _projectService.GetProjects(status);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetJobNameList(string searchString)
        {
                        
            string sugarCrmUrl = this.configuration["SugarCrmUrl"] + "oauth2/token";
            
            using var client = new HttpClient();

            var data = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_id", this.configuration["ClientId"]},
                {"client_secret", this.configuration["CleintSecret"]},
                {"username", this.configuration["CrmUserName"]},
                {"password", this.configuration["CrmPassword"]},
                {"platform", this.configuration["CrmPlatform"]}
            };

            var res = client.PostAsync(sugarCrmUrl, new FormUrlEncodedContent(data));

            var content = res.Result.Content.ReadAsStringAsync();

            var contentJson = JsonConvert.DeserializeObject<dynamic>(content.Result);

            if (contentJson != null)
            {
                foreach (var obj in contentJson)
                {
                    client.DefaultRequestHeaders.Add("Authorization","Bearer " + obj.Value.Value);
                    
                    break;
                }
            }

            var jsObject = new Root()
            {
                name = new Name()
                {
                    starts = searchString
                }
            };

            var jsonData = JsonConvert.SerializeObject(jsObject);


            var uri = this.configuration["SugarCrmUrl"] + "Opportunities?filter=[" + jsonData + "]";

            var opp = client.GetAsync(uri).Result.Content.ReadAsStringAsync();

            var returnVal = JsonConvert.DeserializeObject<OppertunityDto>(opp.Result);

            return Ok(returnVal?.records);
        }

        

        public class Root
        {
            public Name name { get; set; }
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
            model.ProjectDto = _projectService.GetProjectById(id);
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

            int i = 1;
            var wd = "row" + i + "WD";
            var projectDetails = new List<ProjectDetailDto>();
            TempData["TabNumber"] = Request.Form["TabNumber"];
            TempData.Keep("TabNumber");
            var project = new ProjectDto();
            project.ProjectId = Convert.ToInt32(Request.Form["projectId"]);
            project.ProjectName = Request.Form["projectIdForProjectTab"];
            project.NominalCF = Request.Form["nominalCFForFinalTab"];
            project.ActualCF = Request.Form["actualCfForFinalTab"];
            project.LineItemTotal = Request.Form["LineItemTotal"];
            project.Notes = Request.Form["notesForProjectTab"];
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
            var model = new HomeEditViewModel();
            model.ActiveTab = (string)Request.Form["TabNumber"];
            model.ProjectDto = _projectService.GetProjectById(project.ProjectId);
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


        public class Name
        {
            [JsonProperty("$contains")]
            public string starts { get; set; }
        }
    }

    public class OppertunityDto
    {
        public int next_offset { get; set; }

        public List<Oppertunity> records { get; set; }
    }

    public class Oppertunity
    {

        public string Id { get; set; }
        public string Name { get; set; }

        public string Account_name { get; set; }
        public string DisplayName => Name + " (" + Account_name + ")";
    }
}