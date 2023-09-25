using Microsoft.AspNetCore.Http;

namespace Core.Model.Request
{
    public class PlanElevationTextRequest : BaseModelEntity
    {

        public int Id { get; set; }

        public string PlanElevationText { get; set; }

        public IFormFile ImageSnip { get; set; }

        public byte[] PageRef { get; set; }
    }
}
