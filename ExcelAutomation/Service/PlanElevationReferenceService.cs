using ExcelAutomation.Data;
using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public class PlanElevationReferenceService : IPlanElevationReferenceService
    {
        private readonly ExcelAutomationContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public PlanElevationReferenceService(ExcelAutomationContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public int SaveGroup(PlanElevationReferenceDto pElevation, int projectDetailId)
        {
            var dbObject = new PlanElevationReferance();
            dbObject.PlanElevationValue = pElevation.PlanElevationValue;
            dbObject.LFValue = pElevation.LFValue;
            dbObject.ProjectDetailId = projectDetailId;
            dbObject.ImagePath = "PlanElevation/" + pElevation.ImagePath;
            _context.Add(dbObject);
            return _context.SaveChanges();
        }
    }
}
