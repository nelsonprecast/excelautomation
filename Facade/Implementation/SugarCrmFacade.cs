using Core.Domain.ViewOnly;
using Facade.Interfaces;
using Service.Interfaces;

namespace Facade.Implementation
{
    public class SugarCrmFacade : ISugarCrmFacade
    {
        private readonly ISugarCrmService _sugarCrmService;

        public SugarCrmFacade(ISugarCrmService sugarCrmService) {
            _sugarCrmService = sugarCrmService;
        }

        public SugarCrmOppertunityList SearchOppertunities(string searchString)
        {
            return _sugarCrmService.SearchOppertunities(searchString);
        }
    }
}
