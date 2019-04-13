using AliasPro.API.Navigator.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator
{
    internal class NavigatorRepository
    {
        private readonly NavigatorDao _navigatorDao;
        
        public IDictionary<string, ICollection<INavigatorCategory>> _categories { get; private set; }

        public NavigatorRepository(NavigatorDao navigatorDao)
        {
            _navigatorDao = navigatorDao;
            _categories = new Dictionary<string, ICollection<INavigatorCategory>>();

            InitializeNavigator();
        }

        public async void InitializeNavigator()
        {
            _categories.Clear();

            _categories = await _navigatorDao.GetNavigatorCategoriesAsync();
        }

        public bool TryGetCategories(string type, out ICollection<INavigatorCategory> categories) =>
            _categories.TryGetValue(type, out categories);
    }
}
