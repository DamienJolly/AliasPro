using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator
{
    internal class NavigatorController : INavigatorController
    {
        private readonly NavigatorRepository _navigatorRepository;
        
        public NavigatorController(NavigatorRepository navigatorRepository)
        {
            _navigatorRepository = navigatorRepository;
        }
        
        public bool TryGetCategories(string type, out IDictionary<uint, INavigatorCategory> categories) =>
            _navigatorRepository.TryGetCategories(type, out categories);

        public bool TryGetRoomCategory(uint categoryId, out INavigatorCategory category) =>
            _navigatorRepository.TryGetRoomCategory(categoryId, out category);
    }
}
