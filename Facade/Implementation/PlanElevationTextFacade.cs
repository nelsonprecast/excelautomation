using Core.Domain;
using Core.Model.Request;
using Facade.Extension;
using Facade.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

namespace Facade.Implementation
{
    public class PlanElevationTextFacade : IPlanElevationTextFacade
    {
        private readonly IPlanElevationTextService _planElevationTextService;
        private readonly IHostEnvironment _hostEnvironment;

        public PlanElevationTextFacade(IPlanElevationTextService planElevationTextService, IHostEnvironment hostEnvironment)
        {
            _planElevationTextService = planElevationTextService;
            _hostEnvironment = hostEnvironment;
        }

        public ICollection<PlanElevationText> GetPlanElevationTextByProjectId(int projectId)
        {
            return _planElevationTextService.GetPlanElevationTextByProjectId(projectId);
        }

        public void Save(PlanElevationText planElevationText)
        {
            _planElevationTextService.Save(planElevationText);
        }

        public void Save(ICollection<PlanElevationTextRequest> planElevationTextRequests,ICollection<IFormFile> imageSnipFiles,ICollection<IFormFile> pageRefFiles)
        {
            var planElevationTextObjects = planElevationTextRequests.ToEntity<PlanElevationText>();
            foreach (var planElevationText in planElevationTextObjects)
            {
                if (!string.IsNullOrEmpty(planElevationText.ImageSnipPath))
                {
                    IFormFile file = imageSnipFiles.FirstOrDefault(x => x.FileName.Equals(planElevationText.ImageSnipPath));
                    if (file != null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        if (!Directory.Exists(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\PlanElevationText\\ImageSnip\\")))
                        {
                            Directory.CreateDirectory(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\PlanElevationText\\ImageSnip\\"));
                        }
                        var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\PlanElevationText\\ImageSnip\\") + fileName;
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        planElevationText.ImageSnipPath = "/PlanElevationText/ImageSnip/" + fileName; ;
                    }
                }

                if (!string.IsNullOrEmpty(planElevationText.PageRefImagePath))
                {
                    IFormFile file = pageRefFiles.FirstOrDefault(x => x.FileName.Equals(planElevationText.PageRefImagePath));
                    if (file != null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        if (!Directory.Exists(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\PlanElevationText\\PageRef\\")))
                        {
                            Directory.CreateDirectory(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\PlanElevationText\\PageRef\\"));
                        }
                        var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\PlanElevationText\\PageRef\\") + fileName;
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        planElevationText.PageRefImagePath = "/PlanElevationText/PageRef/"+fileName;
                    }
                }
            }

            var existingPlanElevationTextObjects = planElevationTextObjects.Where(x => x.Id > 0);
            var newPlanElevationTextObjects = planElevationTextObjects.Where(x => x.Id == 0);

            var dbPlanElevationTextObjects =
                _planElevationTextService.GetPlanElevationTextByProjectId(existingPlanElevationTextObjects.ElementAt(0)
                    .ProjectId);
            foreach (var dbPlanElevationText in dbPlanElevationTextObjects)
            {
                var exitingPlanElecationText =
                    existingPlanElevationTextObjects.FirstOrDefault(x => x.Id == dbPlanElevationText.Id);
                if (exitingPlanElecationText != null)
                {
                    dbPlanElevationText.Text = exitingPlanElecationText.Text;
                    if (!string.IsNullOrEmpty(exitingPlanElecationText.ImageSnipPath))
                        dbPlanElevationText.ImageSnipPath = exitingPlanElecationText.ImageSnipPath;
                    if (!string.IsNullOrEmpty(exitingPlanElecationText.PageRefImagePath))
                        dbPlanElevationText.PageRefImagePath = exitingPlanElecationText.PageRefImagePath;
                }
            }
            _planElevationTextService.Update(dbPlanElevationTextObjects);
        }

        public PlanElevationText GetPlanElevationTextById(int id)
        {
            return _planElevationTextService.GetPlanElevationTextById(id);
        }
    }
}
