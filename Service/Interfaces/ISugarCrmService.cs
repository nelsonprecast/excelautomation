using Core.Domain;
using Core.Domain.ViewOnly;

namespace Service.Interfaces
{
    public interface ISugarCrmService
    {
        string GetToken();

        string CreateProductTemplate(string name);

        string CreateOppertunities(Project project);

        string CreateProduct(ProjectDetail projectDetail, string productTemplateId,string oppertunityId);

        SugarCrmOppertunityList SearchOppertunities(string searchString);
    }
}
