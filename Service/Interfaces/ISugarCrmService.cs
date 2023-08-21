using Core.Domain;
using Core.Domain.ViewOnly;

namespace Service.Interfaces
{
    public interface ISugarCrmService
    {
        string GetToken();

        string CreateProductTemplate(string token,string name);

        string CreateOppertunities(string token, Project project);

        string CreateProduct(string token, ProjectDetail projectDetail, string productTemplateId);

        SugarCrmOppertunityList SearchOppertunities(string searchString);

        void ConvertProductToQuotes(string token, ICollection<string> productIds, string oppertunityId);
    }
}
