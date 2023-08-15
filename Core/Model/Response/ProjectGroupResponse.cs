namespace Core.Model.Response
{
    public class ProjectGroupResponse : BaseModelEntity
    {
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public List<int> ProjectDetailIds { get; set; }
    }
}
