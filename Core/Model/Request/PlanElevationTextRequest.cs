using Microsoft.AspNetCore.Http;

namespace Core.Model.Request
{
    public class PlanElevationTextRequest : BaseModelEntity
    {

        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Text { get; set; }

        public string ImageSnipPath { get; set; }

        public string PageRefImagePath { get; set; }
    }
}
