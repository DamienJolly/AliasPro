using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator
{
    internal class NavigatorController : INavigatorController
    {
        private readonly NavigatorDao _navigatorDao;

        private IDictionary<string, INavigatorCategory> _categories;
        private readonly IList<string> _views;

        public NavigatorController(NavigatorDao navigatorDao)
        {
            _navigatorDao = navigatorDao;
            _categories = new Dictionary<string, INavigatorCategory>();

            _views = new List<string>
            {
                "official_view",
                "hotel_view",
                "myworld_view"
            };

            InitializeNavigator();
        }

        public async void InitializeNavigator()
        {
            _categories.Clear();

            _categories = await _navigatorDao.GetNavigatorCategoriesAsync();
        }

        public bool IsView(string viewName) =>
            _views.Contains(viewName);

        public bool TryGetCategory(string name, out INavigatorCategory category) =>
            _categories.TryGetValue(name, out category);

        public IList<INavigatorCategory> TryGetCategoryByView(string viewName)
        {
            IList<INavigatorCategory> categories = new List<INavigatorCategory>();
            foreach (INavigatorCategory category in _categories.Values)
            {
                if (category.View == viewName)
                    categories.Add(category);
            }
            return categories;
        }
    }
}
