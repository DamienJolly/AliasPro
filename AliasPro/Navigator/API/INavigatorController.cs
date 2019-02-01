using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator
{
    using Models;

    public interface INavigatorController
    {
        Task<IList<INavigatorCategory>> GetNavigatorCategoriesAsync();
        Task<IList<INavigatorCategory>> GetEventCategoriesAsync();
    }
}
