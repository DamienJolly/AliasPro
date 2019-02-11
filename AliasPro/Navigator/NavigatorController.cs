using System.Collections.Generic;

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
        
        public bool TryGetCategories(string type, out ICollection<INavigatorCategory> categories) =>
            _navigatorRepository.TryGetCategories(type, out categories);
    }

    public interface INavigatorController
    {
        bool TryGetCategories(string type, out ICollection<INavigatorCategory> categories);
    }
}
