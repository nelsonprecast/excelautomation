namespace Core.Model.Request
{
    public class ProjectGroupRequest : BaseModelEntity
    {
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public List<int> ProjectDetailIds { get; set; }
    }
}
