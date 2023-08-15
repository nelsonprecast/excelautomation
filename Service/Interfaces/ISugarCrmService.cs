namespace Service.Interfaces
{
    public interface ISugarCrmService
    {
        string GetToken();

        string CreateProductTemplate(string name);

        string CreateProduct(string name, string productTemplateId);
    }
}
