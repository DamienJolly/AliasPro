using AliasPro.API.Landing;
using AliasPro.API.Landing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing
{
    internal class LandingController : ILandingController
    {
        private readonly LandingRepository _landingRepository;
        
        public LandingController(LandingRepository landingRepository)
        {
            _landingRepository = landingRepository;
        }

        public async Task<IList<IHallOfFamer>> GetHallOfFamersAsync() =>
            await _landingRepository.GetHallOfFamersAsync();

        public async Task<IList<IArticle>> GetNewsArticlesAsync() =>
            await _landingRepository.GetNewsArticlesAsync();
    }
}
