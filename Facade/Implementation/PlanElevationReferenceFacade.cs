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
            foreach (var palElevationReference in planElevationReferences)
            {
                if (palElevationReference.Id < 0)
                {
                  
                }
                else
                {
                    
                }

            }
        }



        private List<PlanElevationReference>? DoDeserilization(string pElevationJsonArray)
        {
            return JsonConvert.DeserializeObject<List<PlanElevationReference>>(pElevationJsonArray);
        }
    }
}
