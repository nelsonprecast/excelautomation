using Core.Domain;
using Core.Model.Request;
using Core.Model.Response;

namespace ExcelAutomation.Models
{
    public class PdfProposalModel
    {
        public PdfProposalRequest PdfProposal { get; set; }

        public ProjectResponse Project { get; set; }
    }
}
