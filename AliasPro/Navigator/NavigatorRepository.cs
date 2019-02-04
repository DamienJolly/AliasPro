using System.Collections.Generic;

namespace AliasPro.Navigator
{
    using Models;

    internal class NavigatorRepository
    {
        private readonly NavigatorDao _navigatorDao;
        
        public IList<INavigatorCategory> Categories { get; private set; }
        public IList<INavigatorCategory> PromotionCategories { get; private set; }

        public NavigatorRepository(NavigatorDao navigatorDao)
        {
            _navigatorDao = navigatorDao;

            Categories = new List<INavigatorCategory>();
            PromotionCategories = new List<INavigatorCategory>();

            InitializeNavigator();
        }

        private async void InitializeNavigator()
        {
            IList<INavigatorCategory> unorderedCategories =
                await _navigatorDao.GetNavigatorCategoriesAsync();

            foreach (INavigatorCategory navigatorCategory in unorderedCategories)
            {
                switch (navigatorCategory.Category)
                {
                    case "hotel_view":
                        {
                            Categories.Add(navigatorCategory);
                            break;
                        }
                    case "roomads_view":
                        {
                            PromotionCategories.Add(navigatorCategory);
                            break;
                        }
                }
            }
        }
    }
}
