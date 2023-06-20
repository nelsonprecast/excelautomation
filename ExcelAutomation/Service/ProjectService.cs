using System.Collections;
using ExcelAutomation.Data;
using ExcelAutomation.Models;
using Microsoft.CodeAnalysis;

namespace ExcelAutomation.Service
{
    public class ProjectService : IProjectService
    {
        private readonly ExcelAutomationContext _context;

        public ProjectService(ExcelAutomationContext context)
        {
            _context = context;
        }

        public async Task<int> SaveProject(ProjectDto project)
        {
            var dbProject = new Data.Project();
            dbProject.ProjectName = project.ProjectName;
            dbProject.ActualCf = project.ActualCF;
            dbProject.NominalCf = project.NominalCF;
            dbProject.CreatedDate = DateTime.Now;

            foreach(var projectDetail in project.ProjectDetails)
            {
                dbProject.ProjectDetails.Add(new ProjectDetail() { 
                    Wd = projectDetail.WD,
                    ItemName = projectDetail.ItemName,
                    DispositionSpecialNote = projectDetail.DispositionSpecialNote,
                    DetailPage = projectDetail.DetailPage,
                    Height = projectDetail.Height,
                    Length = projectDetail.Length,
                    Width = projectDetail.Width,
                    TakeOffColor = projectDetail.TakeOffColor,
                    Pieces = projectDetail.Pieces,  
                    ImagePath = projectDetail.ImagePath
                });
            }           
            _context.Add(dbProject);
            await _context.SaveChangesAsync();
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
                        CreatedDate = project.CreatedDate ?? DateTime.MinValue
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

            var projectDetails = _context.ProjectDetails.Where(x=>x.ProjectId == projectId).ToList();
            if (projectDetails != null)
            {
                projectDto.ProjectDetails = new List<ProjectDetailDto>();
                foreach (var projectDetail in projectDetails)
                {
                    projectDto.ProjectDetails.Add(new ProjectDetailDto()
                    {
                        ProjectDetailId = projectDetail.ProjectDetailId,
                        WD = projectDetail.Wd,
                        ItemName = projectDetail.ItemName,
                        DispositionSpecialNote = projectDetail.DispositionSpecialNote,
                        DetailPage = projectDetail.DetailPage,
                        TakeOffColor = projectDetail.TakeOffColor,
                        Length = projectDetail.Length,
                        Width = projectDetail.Width,
                        Height = projectDetail.Height,
                        Pieces = projectDetail.Pieces,
                        ImagePath = projectDetail.ImagePath,
                        TotalLf = projectDetail.TotalLf,
                        ActSfcflf = projectDetail.ActSfcflf,
                        ActCfpcs = projectDetail.ActCfpcs,
                        TotalActCf = projectDetail.TotalActCf,
                        NomCflf = projectDetail.NomCflf,
                        NomCfpcs = projectDetail.NomCfpcs,
                        TotalNomCf = projectDetail.TotalNomCf,
                        MoldQty = projectDetail.MoldQty,
                        LineItemCharge = projectDetail.LineItemCharge,
                        TotalActualNominalValue = projectDetail.TotalActualNominalValue,
                        PlanElevation = projectDetail.PlanElevation,
                        Category = projectDetail.Category
                    });
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
                _context.Update(project);
                _context.SaveChanges();
                if (project.ProjectDetails != null)
                {
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
                        dbProjectDetail.PlanElevation = projectDetail.PlanElevation;
                        dbProjectDetail.Category = projectDetail.Category;
                        if(!string.IsNullOrEmpty(projectDetail.ImagePath) )
                            dbProjectDetail.ImagePath = projectDetail.ImagePath;
                        if (dbProjectDetail.ProjectDetailId > 0)
                            _context.Update(dbProjectDetail);
                        else
                            _context.Add(dbProjectDetail);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
