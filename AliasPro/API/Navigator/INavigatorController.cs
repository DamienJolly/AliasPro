using AliasPro.API.Navigator.Models;
using System.Collections.Generic;

namespace AliasPro.API.Navigator
{
    public interface INavigatorController
    {
        bool TryGetCategories(string type, out ICollection<INavigatorCategory> categories);
    }
}
