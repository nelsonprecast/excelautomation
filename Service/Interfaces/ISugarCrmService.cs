using Core.Domain;
using Core.Domain.ViewOnly;

namespace Service.Interfaces
{
    public interface ISugarCrmService
    {
        string GetToken();

        string CreateProductTemplate(string token,string name);

        string CreateOppertunities(string token, Project project);

        string CreateProduct(string token, ProjectDetail projectDetail, string productTemplateId,string oppertunityId);

        SugarCrmOppertunityList SearchOppertunities(string searchString);
    }
}
