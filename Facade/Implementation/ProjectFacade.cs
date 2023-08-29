using Azure.Core;
using Core.Domain;
using Core.Model.Response;
using Facade.Extension;
using Facade.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Service.Interfaces;
using System.Globalization;

namespace Facade.Implementation
{
    public class ProjectFacade : IProjectFacade
    {
        private readonly IProjectService _projectService;
        private readonly IProjectDetailService _projectDetailService;
        private readonly IPlanElevationReferenceService _planElevationReferenceService;
        private readonly IPlanElevationTextService _planElevationTextService;
        private readonly IProjectGroupService _projectGroupService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ISugarCrmService _sugarCrmService;

        public ProjectFacade(IProjectService projectService,IProjectDetailService projectDetailService,IPlanElevationReferenceService planElevationReferenceService
                            ,IPlanElevationTextService planElevationTextService,IProjectGroupService projectGroupService,IHttpContextAccessor httpContextAccessor
                            ,ISugarCrmService sugarCrmService)
        {
            _projectService= projectService;
            _projectDetailService= projectDetailService;
            _planElevationReferenceService= planElevationReferenceService;
            _planElevationTextService = planElevationTextService;
            _projectGroupService= projectGroupService;
            _httpContextAccessor= httpContextAccessor;
            _sugarCrmService = sugarCrmService;
        }

        public ICollection<ProjectResponse> GetProjects(string status)
        {
            var projects = _projectService.GetProjects(status);

            foreach (var project in projects.Where(x=> !x.CreatedDate.HasValue))
            {
                project.CreatedDate = DateTime.MinValue;
            }
            return projects.ToModel<ProjectResponse>();
        }
        
        public ProjectResponse GetProjectById(int projectId)
        {
            var project = _projectService.GetProjectById(projectId);

            var projectResponse = project.ToModel<ProjectResponse>();
            var projectDetails = _projectDetailService.GetProjectDetailByProjectId(projectId);
            {
                var projectGroups =
                    _projectGroupService.GetProjectGroupByIds(projectDetails.Where(x=>x.GroupId.HasValue).Select(x => x.GroupId.Value).ToArray());
                var planElevationReferences =
                    _planElevationReferenceService.GetPlanElevationReferenceByProjectDetailIds(projectDetails
                        .Select(x => x.Id).ToArray());

                projectResponse.ProjectDetails = projectDetails.ToModel<ProjectDetailResponse>();
                
                foreach (var projectDetailResponse in projectResponse.ProjectDetails.Where(x=>x.GroupId.HasValue))
                {
                    

                    projectDetailResponse.GroupName = projectGroups
                        .FirstOrDefault(x => x.Id == projectDetailResponse.GroupId.Value).GroupName;

                    
                }
                foreach (var projectDetailResponse in projectResponse.ProjectDetails)
                {
                    projectDetailResponse.PlanElevationReferences =
                        planElevationReferences.Any(x => x.ProjectDetailId == projectDetailResponse.Id)
                            ? planElevationReferences.Where(x => x.ProjectDetailId == projectDetailResponse.Id)
                                .Select(x => x.ToModel<PlanElevationReferenceResponse>())
                                .ToList()
                            : null;
                    projectDetailResponse.PlanElevationJson = projectDetailResponse.PlanElevationReferences != null ?
                        JsonConvert.SerializeObject(projectDetailResponse.PlanElevationReferences, Formatting.Indented, new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.None,
                            TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            MaxDepth = 1
                        }) : string.Empty;
                }
            }

            var planElevationText = _planElevationTextService.GetPlanElevationTextByProjectId(projectId);
            projectResponse.PlanElevationText = planElevationText.ToModel<PlanElevationTextResponse>();
            return projectResponse;
        }

        public async Task UpdateProject()
        {
            var project = await BuildProject();
            var projectDetails = await BuildProjectDetails();
            var sendToCrm = _httpContextAccessor.HttpContext.Request.Form["SendToCrm"];

            var dbProject = _projectService.GetProjectById(project.Id);
            dbProject.ProjectName = project.ProjectName;
            dbProject.ActualCf = project.ActualCf;
            dbProject.NominalCf = project.NominalCf;
            dbProject.RevisionDate = project.RevisionDate;
            dbProject.ContactSpecs = project.ContactSpecs;
            dbProject.Notes = project.Notes;

            _projectService.UpdateProject(dbProject);
           
            foreach (var projectDetail in projectDetails)
            {
                var dbProjectDetail = _projectDetailService.GetProjectDetailById(projectDetail.Id);
                if(dbProjectDetail == null)
                    dbProjectDetail = new ProjectDetail();
                dbProjectDetail.Wd = projectDetail.Wd;
                dbProjectDetail.ItemName = projectDetail.ItemName;
                dbProjectDetail.DispositionSpecialNote = projectDetail.DispositionSpecialNote;
                dbProjectDetail.DetailPage = projectDetail.DetailPage;
                dbProjectDetail.TakeOffColor = projectDetail.TakeOffColor;
                dbProjectDetail.Length = projectDetail.Length;
                dbProjectDetail.Width = projectDetail.Width;
                dbProjectDetail.Height = projectDetail.Height;
                dbProjectDetail.Pieces = projectDetail.Pieces;
                dbProjectDetail.TotalLf = projectDetail.TotalLf;
                dbProjectDetail.ActSfcflf = projectDetail.ActSfcflf;
                dbProjectDetail.ActCfpcs = projectDetail.ActCfpcs;
                dbProjectDetail.TotalActCf = projectDetail.TotalActCf;
                dbProjectDetail.NomCflf = projectDetail.NomCflf;
                dbProjectDetail.NomCfpcs = projectDetail.NomCfpcs;
                dbProjectDetail.TotalNomCf = projectDetail.TotalNomCf;
                dbProjectDetail.MoldQty = projectDetail.MoldQty;
                dbProjectDetail.LineItemCharge = projectDetail.LineItemCharge;
                dbProjectDetail.ProjectId = project.Id;
                dbProjectDetail.TotalActualNominalValue = projectDetail.TotalActualNominalValue;
                dbProjectDetail.Category = projectDetail.Category;
                if (!string.IsNullOrEmpty(projectDetail.ImagePath))
                    dbProjectDetail.ImagePath = projectDetail.ImagePath;
                if (dbProjectDetail.Id > 0)
                    _projectDetailService.UpdateProjectDetail(dbProjectDetail);
                else
                    _projectDetailService.CreateProjectDetail(dbProjectDetail);
            }

            if(sendToCrm.Equals("1"))
            {
                var token = _sugarCrmService.GetToken();
                //var oppertunityId = _sugarCrmService.CreateOppertunities(token, project);
                
                var productIds = new List<string>();
                foreach (var projectDetail in projectDetails)
                {
                    var id = _sugarCrmService.CreateProductTemplate(token, projectDetail.ItemName + " Catalog");
                    var productId = _sugarCrmService.CreateProduct(token, projectDetail, id);
                    productIds.Add("1bc91188-4655-11ee-b994-02b57938b20d");
                }
                _sugarCrmService.ConvertProductToQuotes(token,productIds, "54f5ce02-4654-11ee-af1e-023ef1e03e82");
            }
        }

        public void SaveProject(Project project)
        {
            _projectService.SaveProject(project);
        }

        public void ChangeProjectsStatus(int[] projectIds, string status)
        {
            var projects = _projectService.GetProjectByIds(projectIds);
            foreach (var project in projects)
            {
                project.Status = status;
            }
            _projectService.ChangeProjectsStatus(projects);
        }

        public int CopyProject(int id)
        {
            var projectId = 0;
            var exitingProject = _projectService.GetProjectById(id);
            if (exitingProject != null)
            {
                var exitingProjectDetails = _projectDetailService.GetProjectDetailByProjectId(id);

                var newProject = new Project();
                newProject.ProjectName = exitingProject.ProjectName + " Copy";
                newProject.ActualCf = exitingProject.ActualCf;
                newProject.NominalCf = exitingProject.NominalCf;
                newProject.CreatedDate = DateTime.Now;
                newProject.LineItemTotal = exitingProject.LineItemTotal;

                _projectService.SaveProject(newProject);
                projectId = newProject.Id;
                foreach (var projectDetail in exitingProjectDetails)
                {
                    var newProjectDetail = new ProjectDetail();
                    newProjectDetail.Wd = projectDetail.Wd;
                    newProjectDetail.ItemName = projectDetail.ItemName;
                    newProjectDetail.DispositionSpecialNote = projectDetail.DispositionSpecialNote;
                    newProjectDetail.DetailPage = projectDetail.DetailPage;
                    newProjectDetail.TakeOffColor = projectDetail.TakeOffColor;
                    newProjectDetail.Length = projectDetail.Length;
                    newProjectDetail.Width = projectDetail.Width;
                    newProjectDetail.Height = projectDetail.Height;
                    newProjectDetail.Pieces = projectDetail.Pieces;
                    newProjectDetail.TotalLf = projectDetail.TotalLf;
                    newProjectDetail.ActSfcflf = projectDetail.ActSfcflf;
                    newProjectDetail.ActCfpcs = projectDetail.ActCfpcs;
                    newProjectDetail.TotalActCf = projectDetail.TotalActCf;
                    newProjectDetail.NomCflf = projectDetail.NomCflf;
                    newProjectDetail.NomCfpcs = projectDetail.NomCfpcs;
                    newProjectDetail.TotalNomCf = projectDetail.TotalNomCf;
                    newProjectDetail.MoldQty = projectDetail.MoldQty;
                    newProjectDetail.LineItemCharge = projectDetail.LineItemCharge;
                    newProjectDetail.ProjectId = newProject.Id;
                    newProjectDetail.TotalActualNominalValue = projectDetail.TotalActualNominalValue;
                    newProjectDetail.Category = projectDetail.Category;
                    newProjectDetail.ImagePath = projectDetail.ImagePath;
                    newProject.ProjectDetails.Add(newProjectDetail);
                }
                _projectDetailService.CreateProjectDetail(newProject.ProjectDetails);
            }

            return projectId;
        }


        public int DeleteProjectDetailRow(int id)
        {
            var projectDetail = _projectDetailService.GetProjectDetailById(id);
            _projectDetailService.DeleteProjectDetail(projectDetail);
            return projectDetail.ProjectId;
        }

        #region Private Methods

        private async Task<Project> BuildProject()
        {
            var Request = _httpContextAccessor.HttpContext.Request;
            var project = new Project();
            project.Id = Convert.ToInt32(Request.Form["projectId"]);
            project.ProjectName = Request.Form["projectIdForProjectTab"];
            project.NominalCf = Request.Form["nominalCFForFinalTab"];
            project.ActualCf = Request.Form["actualCfForFinalTab"];
            project.LineItemTotal = Request.Form["LineItemTotal"];
            project.Notes = Request.Form["notesForProjectTab"];
            if (!string.IsNullOrEmpty(Request.Form["RevisionDate"]))
                project.RevisionDate = DateTime.ParseExact(Request.Form["RevisionDate"], "MM/dd/yyyy",
                    CultureInfo.InvariantCulture);
            if (Request.Form.Files["ContactSpecs"] != null)
            {
                var file = Request.Form.Files["ContactSpecs"];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "ContactSpecs\\") + fileName;
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                project.ContactSpecs = "/ContactSpecs/" + fileName;
            }
            return project;
        }

        private async Task<ICollection<ProjectDetail>> BuildProjectDetails()
        {
            int i = 1;
            var wd = "row" + i + "WD";
            var Request = _httpContextAccessor.HttpContext.Request;
            var projectDetailList = new List<ProjectDetail>();
            while (!string.IsNullOrEmpty(Request.Form[wd]))
            {
                var projectDetail = new ProjectDetail();
                if (!string.IsNullOrEmpty(Request.Form["row" + i + "projectDetailId"]))
                {
                    projectDetail.Id = Convert.ToInt32(Request.Form["row" + i + "projectDetailId"]);
                }
                projectDetail.Wd = Request.Form[wd];
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
                projectDetail.Category = Request.Form["row" + i + "Category"];
                if (!string.IsNullOrEmpty(Request.Form["rowFP" + i + "LineItemCharge"]))
                    projectDetail.LineItemCharge = Request.Form["rowFP" + i + "LineItemCharge"];
                if (!string.IsNullOrEmpty(Request.Form["rowFP" + i + "TotalCheckBox"]))
                    projectDetail.TotalActualNominalValue = Request.Form["rowFP" + i + "TotalCheckBox"];

                if (Request.Form.Files["row" + i + "File"] != null)
                {
                    var file = Request.Form.Files["row" + i + "File"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "ProjectImages\\") + fileName;
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    projectDetail.ImagePath = "/ProjectImages/" + fileName;
                }

                projectDetailList.Add(projectDetail);
                i++;
                wd = "row" + i + "WD";
            }

            return projectDetailList;
        }

        #endregion
    }
}
