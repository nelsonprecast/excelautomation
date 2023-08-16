using Core.Domain.ViewOnly;

namespace Service.Interfaces
{
    public interface ISugarCrmService
    {
        string GetToken();

        string CreateProductTemplate(string name);

        string CreateProduct(string name, string productTemplateId);

        SugarCrmOppertunityList SearchOppertunities(string searchString);
    }
}
