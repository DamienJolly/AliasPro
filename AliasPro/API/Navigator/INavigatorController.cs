using AliasPro.API.Navigator.Models;
using System.Collections.Generic;

namespace AliasPro.API.Navigator
{
    public interface INavigatorController
    {
        void InitializeNavigator();
        bool IsView(string name);
        bool TryGetCategory(string name, out INavigatorCategory category);
        IList<INavigatorCategory> TryGetCategoryByView(string viewName);
    }
}
