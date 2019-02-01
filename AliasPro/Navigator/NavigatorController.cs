using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator
{
    using Models;

    internal class NavigatorController : INavigatorController
    {
        private readonly NavigatorRepository _navigatorRepository;
        
        public NavigatorController(NavigatorRepository navigatorRepository)
        {
            _navigatorRepository = navigatorRepository;
        }

        public async Task<IList<INavigatorCategory>> GetNavigatorCategoriesAsync() =>
            await _navigatorRepository.GetNavigatorCategoriesAsync();

        public async Task<IList<INavigatorCategory>> GetEventCategoriesAsync() =>
            await _navigatorRepository.GetPromotionNavigatorCategoriesAsync();
    }
}
