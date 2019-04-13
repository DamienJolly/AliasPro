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
        
        public bool TryGetCategories(string type, out ICollection<INavigatorCategory> categories) =>
            _navigatorRepository.TryGetCategories(type, out categories);
    }
}
