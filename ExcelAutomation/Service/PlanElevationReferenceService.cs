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

        public ICollection<PlanElevationReferenceDto> GetByProjectDetailId(int projectDetailID)
        {
           return _context.PlanElevationReferances.Where(p=>p.ProjectDetailId==projectDetailID)
                .Select(p => new PlanElevationReferenceDto() { 
                    ProjectDetailId = p.ProjectDetailId,
                    ImagePath = p.ImagePath,    
                    PlanElevationReferanceId = p.PlanElevationReferanceId,
                    LFValue = p.LFValue,
                    PlanElevationValue = p.PlanElevationValue,
                }).ToList();
        }

        public int Save(PlanElevationReferenceDto pElevation, int projectDetailId)
        {
            var dbObject = new PlanElevationReferance();
            dbObject.PlanElevationValue = pElevation.PlanElevationValue;
            dbObject.LFValue = pElevation.LFValue;
            dbObject.ProjectDetailId = projectDetailId;
            dbObject.ImagePath = "PlanElevation/" + pElevation.ImagePath;
            _context.Add(dbObject);
            _context.SaveChanges();
            return dbObject.PlanElevationReferanceId;
        }
    }
}
