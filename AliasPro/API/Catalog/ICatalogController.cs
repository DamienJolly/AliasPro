using AliasPro.API.Catalog.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Catalog
{
    public interface ICatalogController
    {
        ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank);

        bool TryGetCatalogPage(int pageId, out ICatalogPage page);
        void ReloadCatalog();
        Task AddLimitedAsync(uint itemId, uint playerId, int number);
		Task<int> AddNewBotAsync(IPlayerBot playerBot, int playerId);
		Task<int> AddNewPetAsync(IPlayerPet playerPet, int playerId);

		bool TryGetGift(int spriteId, out ICatalogGiftPart item);
		ICollection<ICatalogGiftPart> GetGifts { get; }
		ICollection<ICatalogGiftPart> GetWrappers { get; }
	}
}
