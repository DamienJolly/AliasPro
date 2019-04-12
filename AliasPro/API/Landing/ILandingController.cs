using AliasPro.API.Landing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Landing
{
    public interface ILandingController
    {
        Task<IList<IHallOfFamer>> GetHallOfFamersAsync();
        Task<IList<IArticle>> GetNewsArticlesAsync();
    }
}
