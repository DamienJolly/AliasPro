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

        public IList<INavigatorCategory> Categories =>
            _navigatorRepository.Categories;

        public IList<INavigatorCategory> PromotionCategories =>
            _navigatorRepository.PromotionCategories;
    }

    public interface INavigatorController
    {
        IList<INavigatorCategory> Categories { get; }
        IList<INavigatorCategory> PromotionCategories { get; }
    }
}
