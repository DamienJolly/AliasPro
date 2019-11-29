﻿using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    internal class CatalogController : ICatalogController
    {
        private readonly CatalogRepostiory _catalogRepostiory;

        public CatalogController(CatalogRepostiory catalogRepostiory)
        {
            _catalogRepostiory = catalogRepostiory;
        }

        public bool TryGetCatalogPage(int pageId, out ICatalogPage page) =>
            _catalogRepostiory.TryGetCatalogPage(pageId, out page);

        public ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank) =>
            _catalogRepostiory.GetCatalogPages(pageId, rank);

        public void ReloadCatalog() =>
            _catalogRepostiory.InitializeCatalog();

        public async Task AddLimitedAsync(uint itemId, uint playerId, int number) =>
            await _catalogRepostiory.AddLimitedAsync(itemId, playerId, number);

		public async Task<int> AddNewBotAsync(IPlayerBot playerBot, int playerId) =>
			await _catalogRepostiory.AddNewBotAsync(playerBot, playerId);

		public bool TryGetGift(int spriteId, out ICatalogGiftPart item) =>
			_catalogRepostiory.TryGetGift(spriteId, out item);

		public ICollection<ICatalogGiftPart> GetGifts =>
			_catalogRepostiory.GetGifts;

		public ICollection<ICatalogGiftPart> GetWrappers =>
			_catalogRepostiory.GetWrappers;
	}
}