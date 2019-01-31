using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing
{
    using Models;

    public interface ILandingController
    {
        Task<IList<IHallOfFamer>> GetHallOfFamersAsync();
        Task<IList<IArticles>> GetNewsArticlesAsync();
    }
}
