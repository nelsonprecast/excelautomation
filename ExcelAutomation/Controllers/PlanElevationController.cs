using ExcelAutomation.Models;
using ExcelAutomation.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExcelAutomation.Controllers
{
    public class PlanElevationController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPlanElevationReferenceService _planElevationReferenceService;

        public PlanElevationController(IWebHostEnvironment webHostEnvironment,IPlanElevationReferenceService planElevationReferenceService)
        {
            _webHostEnvironment= webHostEnvironment;
            _planElevationReferenceService= planElevationReferenceService;
        }


        [HttpPost]
        public JsonResult UploadImages(ICollection<IFormFile> files, ICollection<IFormFile> ifiles, int projectDetailId, string pElevationJsonArray)
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
            foreach (var pElevation in pElevationList)
            {
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



        #region Private Methods

        private List<PlanElevationReferenceDto> DoDeserilization(string pElevationJsonArray)
        {
            return JsonConvert.DeserializeObject<List<PlanElevationReferenceDto>>(pElevationJsonArray);
        }

        #endregion
    }
}
