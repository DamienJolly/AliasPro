using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Catalog
{
    public interface ICatalogController
    {
        ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank);
		ICollection<ICatalogFeaturedPage> GetCatalogFeaturedPages { get; }

		bool TryGetCatalogPage(int pageId, out ICatalogPage page);
        void InitializeCatalog();
        Task AddLimitedAsync(uint itemId, uint playerId, int number);
		Task<int> AddNewBotAsync(IPlayerBot playerBot, int playerId);
		Task<int> AddNewPetAsync(IPlayerPet playerPet, int playerId);

		bool TryGetGift(int spriteId, out ICatalogGiftPart item);
		ICollection<ICatalogGiftPart> GetGifts { get; }
		ICollection<ICatalogGiftPart> GetWrappers { get; }
		IDictionary<int, IList<IItemData>> GetRecyclerPrizes { get; }
		IDictionary<int, int> GetRecyclerLevels { get; }
		IItemData RecyclerPrize { get; }
	}
}
