namespace Core.Model.Request
{
    public class PdfProposalRequest : BaseModelEntity
    {
        public int ProjectId { get; set; }
        public string CustomerName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
