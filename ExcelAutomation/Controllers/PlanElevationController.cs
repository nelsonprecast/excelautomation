using ExcelAutomation.Models;
using Facade.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExcelAutomation.Controllers
{
    public class PlanElevationController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPlanElevationReferenceFacade _planElevationReferenceFacade;

        public PlanElevationController(IWebHostEnvironment webHostEnvironment,IPlanElevationReferenceFacade planElevationReferenceFacade)
        {
            _webHostEnvironment= webHostEnvironment;
            _planElevationReferenceFacade= planElevationReferenceFacade;
        }


        [HttpPost]
        public JsonResult UploadImages(ICollection<IFormFile> files, ICollection<IFormFile> ifiles, int projectDetailId, string pElevationJsonArray)
        {
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "PlanElevation");
            _planElevationReferenceFacade.SavePlanElevationReference(files,ifiles,projectDetailId,pElevationJsonArray);
            
            return new JsonResult(JsonConvert.SerializeObject(_planElevationReferenceFacade.GetPlanElevationReferenceByProjectDetailId(projectDetailId)));           
        }



        #region Private Methods

        private List<PlanElevationReferenceDto> DoDeserilization(string pElevationJsonArray)
        {
            return JsonConvert.DeserializeObject<List<PlanElevationReferenceDto>>(pElevationJsonArray);
        }

        #endregion
    }
}
