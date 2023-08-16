using Core.Domain;
using Core.Domain.ViewOnly;

namespace Service.Interfaces
{
    public interface ISugarCrmService
    {
        string GetToken();

        string CreateProductTemplate(string name);

        string CreateProduct(ProjectDetail projectDetail, string productTemplateId);

        SugarCrmOppertunityList SearchOppertunities(string searchString);
    }
}
