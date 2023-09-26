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
        private readonly IPlanElevationTextFacade _planElevationTextFacade;
        public PlanElevationController(IWebHostEnvironment webHostEnvironment,IPlanElevationReferenceFacade planElevationReferenceFacade,IPlanElevationTextFacade planElevationTextFacade)
        {
            _webHostEnvironment= webHostEnvironment;
            _planElevationReferenceFacade= planElevationReferenceFacade;
            _planElevationTextFacade = planElevationTextFacade;
        }


        [HttpPost]
        public JsonResult UploadImages(ICollection<IFormFile> files, ICollection<IFormFile> ifiles, int projectDetailId, string pElevationJsonArray)
        {
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "PlanElevation");
            _planElevationReferenceFacade.SavePlanElevationReference(files,ifiles,projectDetailId,pElevationJsonArray);
            
            return new JsonResult(JsonConvert.SerializeObject(_planElevationReferenceFacade.GetPlanElevationReferenceByProjectDetailId(projectDetailId)));           
        }

        public JsonResult GetPlanElevationTextById(int id)
        {
            return new JsonResult(_planElevationTextFacade.GetPlanElevationTextById(id));
        }

        #region Private Methods

        private List<PlanElevationReferenceDto> DoDeserilization(string pElevationJsonArray)
        {
            return JsonConvert.DeserializeObject<List<PlanElevationReferenceDto>>(pElevationJsonArray);
        }

        #endregion
    }
}
