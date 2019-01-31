using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing
{
    using Models;

    internal class LandingController : ILandingController
    {
        private readonly LandingRepository _landingRepository;
        
        public LandingController(LandingRepository landingRepository)
        {
            _landingRepository = landingRepository;
        }

        public async Task<IList<IHallOfFamer>> GetHallOfFamersAsync() =>
            await _landingRepository.GetHallOfFamersAsync();

        public async Task<IList<IArticles>> GetNewsArticlesAsync() =>
            await _landingRepository.GetNewsArticlesAsync();
    }
}
