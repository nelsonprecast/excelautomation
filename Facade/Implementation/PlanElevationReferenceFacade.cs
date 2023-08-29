using Core.Domain;
using Facade.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Service.Interfaces;

namespace Facade.Implementation
{
    public class PlanElevationReferenceFacade : IPlanElevationReferenceFacade
    {
        private readonly IPlanElevationReferenceService _planElevationReferenceService;
        private readonly IHostEnvironment _hostEnvironment;

        public PlanElevationReferenceFacade(IPlanElevationReferenceService planElevationReferenceService, IHostEnvironment hostEnvironment)
        {
            _planElevationReferenceService = planElevationReferenceService;
            _hostEnvironment= hostEnvironment;
        }

        public void SavePlanElevationReference(ICollection<IFormFile> files, ICollection<IFormFile> ifiles,
            int projectDetailId, string planElevationReferenceJson)
        {
            var uploads = Path.Combine(_hostEnvironment.ContentRootPath, "PlanElevation");
            if(!Directory.Exists(uploads)) {
                Directory.CreateDirectory(uploads);
            }
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
            var planElevationReferences = DoDeserilization(planElevationReferenceJson);
            var exitingPlanElevationReferences = _planElevationReferenceService.GetPlanElevationReferenceByProjectDetailId(projectDetailId);
            foreach (var planElevationReference in planElevationReferences)
            {
                if (planElevationReference.Id < 1) // Create
                {
                    planElevationReference.Id = 0;
                    planElevationReference.ProjectDetailId = projectDetailId;
                    _planElevationReferenceService.CreatePlanElevationReference(planElevationReference);
                } 
                else   // Update
                {
                    var exitingPlanElevationReference = exitingPlanElevationReferences.FirstOrDefault(x=>x.Id == planElevationReference.Id);
                    if(exitingPlanElevationReferences != null)
                    {
                        exitingPlanElevationReference.PlanElevationValue = planElevationReference.PlanElevationValue;
                        exitingPlanElevationReference.PcsValue = planElevationReference.PcsValue;
                        exitingPlanElevationReference.LFValue = planElevationReference.LFValue;
                        exitingPlanElevationReference.ImagePath = planElevationReference.ImagePath;
                        exitingPlanElevationReference.PageRefPath = planElevationReference.PageRefPath;

                        _planElevationReferenceService.UpdatePlanElevationReference(exitingPlanElevationReference);
                    }
                }

            }
        }

        public ICollection<PlanElevationReference> GetPlanElevationReferenceByProjectDetailId(int projectDetailId)
        {
            return _planElevationReferenceService.GetPlanElevationReferenceByProjectDetailId(projectDetailId);
        }


        private List<PlanElevationReference>? DoDeserilization(string pElevationJsonArray)
        {
            return JsonConvert.DeserializeObject<List<PlanElevationReference>>(pElevationJsonArray);
        }
    }
}
