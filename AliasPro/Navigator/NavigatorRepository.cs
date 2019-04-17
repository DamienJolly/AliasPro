using AliasPro.API.Navigator.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator
{
    internal class NavigatorRepository
    {
        private readonly NavigatorDao _navigatorDao;
        
        public IDictionary<string, IDictionary<uint, INavigatorCategory>> _categories { get; private set; }

        public NavigatorRepository(NavigatorDao navigatorDao)
        {
            _navigatorDao = navigatorDao;
            _categories = new Dictionary<string, IDictionary<uint, INavigatorCategory>>();

            InitializeNavigator();
        }

        public async void InitializeNavigator()
        {
            _categories.Clear();

            _categories = await _navigatorDao.GetNavigatorCategoriesAsync();
        }

        public bool TryGetCategories(string type, out IDictionary<uint, INavigatorCategory> categories) =>
            _categories.TryGetValue(type, out categories);

        public bool TryGetRoomCategory(uint categoryId, out INavigatorCategory category)
        {
            category = null;
            if (!_categories.TryGetValue("hotel_view", out IDictionary<uint, INavigatorCategory> categories))
                return false;
            
            return categories.TryGetValue(categoryId, out category);
        }
    }
}
