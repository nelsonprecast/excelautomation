using ExcelAutomation.Data;
using ExcelAutomation.Models;
using Newtonsoft.Json;


namespace ExcelAutomation.Service
{
    public class ProjectService : IProjectService
    {
        private readonly ExcelAutomationContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public ProjectService(ExcelAutomationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        public async Task<int> SaveProject(ProjectDto project)
        {
            var dbProject = new Data.Project();
            dbProject.ProjectName = project.ProjectName;
            dbProject.ActualCf = project.ActualCF;
            dbProject.NominalCf = project.NominalCF;
            dbProject.CreatedDate = DateTime.Now;
            dbProject.ContactSpecs = project.ContactSpecs;

            _context.Add(dbProject);
            await _context.SaveChangesAsync();
            var pdIndex = 1;
            foreach (var projectDetail in project.ProjectDetails)
            {
                var dbProjectDetail = new ProjectDetail();
                dbProjectDetail.ProjectId = dbProject.ProjectId;
                dbProjectDetail.Wd = projectDetail.WD;
                dbProjectDetail.ItemName = projectDetail.ItemName;
                dbProjectDetail.DispositionSpecialNote = projectDetail.DispositionSpecialNote;
                dbProjectDetail.DetailPage = projectDetail.DetailPage;
                dbProjectDetail.Height = projectDetail.Height;
                dbProjectDetail.Length = projectDetail.Length;
                dbProjectDetail.Width = projectDetail.Width;
                dbProjectDetail.TakeOffColor = projectDetail.TakeOffColor;
                dbProjectDetail.Pieces = projectDetail.Pieces;
                dbProjectDetail.ImagePath = projectDetail.ImagePath;
                dbProjectDetail.TotalLf = projectDetail.TotalLf;
                dbProjectDetail.ActSfcflf = projectDetail.ActSfcflf;
                dbProjectDetail.ActCfpcs = projectDetail.ActCfpcs;
                dbProjectDetail.TotalActCf = projectDetail.TotalActCf;
                dbProjectDetail.NomCflf = projectDetail.NomCflf;
                dbProjectDetail.NomCfpcs = projectDetail.NomCfpcs;
                dbProjectDetail.TotalNomCf = projectDetail.TotalNomCf;
                dbProjectDetail.MoldQty = projectDetail.MoldQty;
                dbProjectDetail.LineItemCharge = projectDetail.LineItemCharge;
                dbProjectDetail.TotalActualNominalValue = projectDetail.TotalActualNominalValue;
                dbProjectDetail.Category = projectDetail.Category;

                _context.Add(dbProjectDetail);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(projectDetail.PlanElevation))
                {
                    var planElevationArray = projectDetail.PlanElevation.Split("@_@");
                    var lfValueArray = projectDetail.LFValue.Split("@_@");

                    for (int i = 0; i < planElevationArray.Length; i++)
                    {
                        var dbfilePath = string.Empty;
                        var file = projectDetail.PlanElevationFiles.FirstOrDefault(x =>
                            x.Name.Equals($"hiddenPlanElevationFile{pdIndex}_{i + 1}"));
                        if (file != null)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "PlanElevation\\") + fileName;
                            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                dbfilePath = "/PlanElevation/" + fileName;
                            }
                        }
                        _context.Add(new PlanElevationReferance()
                        {
                            ProjectDetailId = dbProjectDetail.ProjectDetailId,
                            LFValue = lfValueArray[i],
                            PlanElevationValue = planElevationArray[i],
                            ImagePath = dbfilePath
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                pdIndex++;
            }           
            
            
            return dbProject.ProjectId;
        }

        public ICollection<ProjectDto> GetProjects()
        {
            ICollection<ProjectDto> projectDtos = new List<ProjectDto>();
            var projects =  _context.Projects.ToList();
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    projectDtos.Add(new ProjectDto()
                    {
                        ProjectId = project.ProjectId,
                        ProjectName = project.ProjectName,
                        ActualCF = project.ActualCf,
                        NominalCF = project.NominalCf,
                        CreatedDate = project.CreatedDate ?? DateTime.MinValue,
                        ContactSpecs = project.ContactSpecs,
                        RevisionDate = project.RevisionDate
                    });
                }
            }

            return projectDtos;
        }

        public ProjectDto GetProjectById(int projectId)
        {
            var project = _context.Projects.SingleOrDefault(x => x.ProjectId == projectId);
            var projectDto = new ProjectDto();
            projectDto.ProjectId = projectId;
            projectDto.ProjectName = project.ProjectName;
            projectDto.ActualCF = project.ActualCf;
            projectDto.NominalCF = project.NominalCf;
            projectDto.CreatedDate = project.CreatedDate ?? DateTime.MinValue;
            projectDto.LineItemTotal = project.LineItemTotal;
            projectDto.RevisionDate = project.RevisionDate;
            projectDto.ContactSpecs = project.ContactSpecs;

            var projectDetails = _context.ProjectDetails.Where(x=>x.ProjectId == projectId).ToList();
            if (projectDetails != null)
            {
                var projectDetailIds = projectDetails.Select(x => x.ProjectDetailId).ToArray();
                var planElevationReferences = _context.PlanElevationReferances
                    .Where(x => projectDetailIds.Contains(x.ProjectDetailId)).ToList();
                projectDto.ProjectDetails = new List<ProjectDetailDto>();
                
                foreach (var projectDetail in projectDetails)
                {
                    var projectDetailDto = new ProjectDetailDto();

                    projectDetailDto.ProjectDetailId = projectDetail.ProjectDetailId;
                    projectDetailDto.WD = projectDetail.Wd;
                        projectDetailDto.ItemName = projectDetail.ItemName;
                    projectDetailDto.DispositionSpecialNote = projectDetail.DispositionSpecialNote;
                    projectDetailDto.DetailPage = projectDetail.DetailPage;
                    projectDetailDto.TakeOffColor = projectDetail.TakeOffColor;
                    projectDetailDto.Length = projectDetail.Length;
                    projectDetailDto.Width = projectDetail.Width;
                    projectDetailDto.Height = projectDetail.Height;
                    projectDetailDto.Pieces = projectDetail.Pieces;
                    projectDetailDto.ImagePath = projectDetail.ImagePath;
                    projectDetailDto.TotalLf = projectDetail.TotalLf;
                    projectDetailDto.ActSfcflf = projectDetail.ActSfcflf;
                    projectDetailDto.ActCfpcs = projectDetail.ActCfpcs;
                    projectDetailDto.TotalActCf = projectDetail.TotalActCf;
                    projectDetailDto.NomCflf = projectDetail.NomCflf;
                    projectDetailDto.NomCfpcs = projectDetail.NomCfpcs;
                    projectDetailDto.TotalNomCf = projectDetail.TotalNomCf;
                    projectDetailDto.MoldQty = projectDetail.MoldQty;
                    projectDetailDto.LineItemCharge = projectDetail.LineItemCharge;
                    projectDetailDto.TotalActualNominalValue = projectDetail.TotalActualNominalValue;
                    projectDetailDto.Category = projectDetail.Category;
                    projectDetailDto.PlanElevationJson = planElevationReferences.Any(x => x.ProjectDetailId == projectDetail.ProjectDetailId) 
                        ? JsonConvert.SerializeObject(planElevationReferences.Where(x => x.ProjectDetailId == projectDetail.ProjectDetailId)
                            .Select(x=>new PlanElevationReferenceDto()
                            {
                                ProjectDetailId = x.ProjectDetailId,
                                PlanElevationReferanceId = x.PlanElevationReferanceId,
                                ImagePath = x.ImagePath,
                                LFValue = x.LFValue,
                                PlanElevationValue = x.PlanElevationValue
                            })
                            .ToList(), Formatting.Indented, new JsonSerializerSettings()
                            {
                                TypeNameHandling = TypeNameHandling.Objects,
                                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                                MaxDepth = 1
                            }) :string.Empty;
                    projectDetailDto.PlanElevation = (planElevationReferences.Any(x=>x.ProjectDetailId == projectDetail.ProjectDetailId) ? string.Join("@_@", planElevationReferences.Where(x => x.ProjectDetailId == projectDetail.ProjectDetailId).Select(x=>x.PlanElevationValue)):string.Empty);
                    projectDetailDto.LFValue = (planElevationReferences.Any(x => x.ProjectDetailId == projectDetail.ProjectDetailId) ? string.Join("@_@", planElevationReferences.Where(x => x.ProjectDetailId == projectDetail.ProjectDetailId).Select(x => x.LFValue + "_" + x.ImagePath)) : string.Empty);
                    projectDto.ProjectDetails.Add(projectDetailDto);
                }
            }

            return projectDto;
        }

        public void UpdateProjectDetail(ProjectDto projectDto)
        {
            var project = _context.Projects.SingleOrDefault(x => x.ProjectId == projectDto.ProjectId);
            if (project != null)
            {
                project.ProjectName = projectDto.ProjectName;
                project.ActualCf = projectDto.ActualCF;
                project.NominalCf = projectDto.NominalCF;
                project.LineItemTotal = projectDto.LineItemTotal;
                project.RevisionDate = projectDto.RevisionDate;
                project.ContactSpecs = projectDto.ContactSpecs;

                _context.Update(project);
                _context.SaveChanges();
                if (project.ProjectDetails != null)
                {
                    var pdIndex = 1;
                    foreach (var projectDetail in projectDto.ProjectDetails)
                    {
                        var dbProjectDetail =
                            _context.ProjectDetails.FirstOrDefault(x =>
                                x.ProjectDetailId == projectDetail.ProjectDetailId);
                        if (dbProjectDetail == null)
                            dbProjectDetail = new ProjectDetail();
                        dbProjectDetail.Wd = projectDetail.WD;
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
                        dbProjectDetail.ProjectId = project.ProjectId;
                        dbProjectDetail.TotalActualNominalValue = projectDetail.TotalActualNominalValue;
                        dbProjectDetail.Category = projectDetail.Category;
                        if(!string.IsNullOrEmpty(projectDetail.ImagePath) )
                            dbProjectDetail.ImagePath = projectDetail.ImagePath;
                        if (dbProjectDetail.ProjectDetailId > 0)
                            _context.Update(dbProjectDetail);
                        else
                            _context.Add(dbProjectDetail);
                        
                        _context.SaveChanges();


                        var existingList = _context.PlanElevationReferances.Where(x =>
                            x.ProjectDetailId == dbProjectDetail.ProjectDetailId).ToList();
                        

                        if (projectDetail.PlanElevationReferences != null && projectDetail.PlanElevationReferences.Count > 0)
                        {
                            var i = 1;
                            foreach (var planElevationObj in projectDetail.PlanElevationReferences)
                            {
                                var dbfilePath = string.Empty;
                                var file = projectDetail.PlanElevationFiles.FirstOrDefault(x =>
                                    x.Name.Equals($"hiddenPlanElevationFile{pdIndex}_{i}"));
                                if (file != null)
                                {
                                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "PlanElevation\\") + fileName;
                                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                                    {
                                        file.CopyTo(fileStream);
                                        dbfilePath = "/PlanElevation/" + fileName;
                                    }
                                }

                                var dbPlanElevation = existingList.FirstOrDefault(x => x.PlanElevationReferanceId == planElevationObj.PlanElevationReferanceId);
                                if (dbPlanElevation != null)
                                {
                                    dbPlanElevation.LFValue = planElevationObj.LFValue;
                                    dbPlanElevation.PlanElevationValue = planElevationObj.PlanElevationValue;
                                    dbPlanElevation.ProjectDetailId = dbProjectDetail.ProjectDetailId;
                                    if (!string.IsNullOrEmpty(dbfilePath))
                                        dbPlanElevation.ImagePath = dbfilePath;
                                    _context.Update(dbPlanElevation);
                                }
                                else
                                {
                                    _context.Add(new PlanElevationReferance()
                                    {
                                        ProjectDetailId = dbProjectDetail.ProjectDetailId,
                                        LFValue = planElevationObj.LFValue,
                                        PlanElevationValue = planElevationObj.PlanElevationValue,
                                        ImagePath = dbfilePath
                                    });
                                }
                                i++;
                            }

                            _context.SaveChanges();
                        }

                        pdIndex++;
                    }
                }
            }
        }

        public int DeleteProjectDetailRow(int id)
        {
            var projectDetail = _context.ProjectDetails.FirstOrDefault(x => x.ProjectDetailId == id);
            if(projectDetail  != null)
            {
                _context.Remove(projectDetail);
                _context.SaveChanges();
            }

            return projectDetail.ProjectId;
        }

        public int CopyProject(int id)
        {
            var project = _context.Projects.FirstOrDefault(x => x.ProjectId == id);
            var newProject = new Data.Project();
            newProject.ProjectName = project.ProjectName+" Copy";
            newProject.ActualCf = project.ActualCf;
            newProject.NominalCf = project.NominalCf;
            newProject.CreatedDate = DateTime.Now;
            newProject.LineItemTotal = project.LineItemTotal;
            ;
            
            var projectDetails = _context.ProjectDetails.Where(x => x.ProjectId == project.ProjectId);
            foreach (var projectDetail in projectDetails)
            {
                var newProjectDetail = new Data.ProjectDetail();
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
                newProjectDetail.ProjectId = project.ProjectId;
                newProjectDetail.TotalActualNominalValue = projectDetail.TotalActualNominalValue;
                newProjectDetail.Category = projectDetail.Category;
                newProjectDetail.ImagePath = projectDetail.ImagePath;
                newProject.ProjectDetails.Add(newProjectDetail);
            }

            _context.Add(newProject);
            _context.SaveChanges();
            return newProject.ProjectId;
        }
    }
}
