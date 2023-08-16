using Core.Domain.ViewOnly;

namespace Facade.Interfaces
{
    public interface ISugarCrmFacade
    {
        SugarCrmOppertunityList SearchOppertunities(string searchString);
    }
}
