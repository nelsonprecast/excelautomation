using Core.Domain;
using Core.Domain.ViewOnly;

namespace Service.Interfaces
{
    public interface ISugarCrmService
    {
        string GetToken();

        string CreateProductTemplate(string token,string name);

        string CreateOpportunity(string token, Project project);

        string CreateProduct(string token, ProjectDetail projectDetail, string productTemplateId);

        dynamic GetProduct(ProjectDetail projectDetail, string productTemplateId);

        string ConvertQuotes(string token, Project project, string oppertunityId);

        string CreateQuote(string token, string quoteName, ICollection<dynamic> _products, string opportunityId);

        SugarCrmOppertunityList SearchOppertunities(string searchString);

        void ConvertProductToQuotes(string token, ICollection<string> productIds, string oppertunityId);
    }
}
