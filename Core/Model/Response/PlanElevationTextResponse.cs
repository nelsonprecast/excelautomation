namespace Core.Model.Response
{
    public class PlanElevationTextResponse : BaseModelEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
