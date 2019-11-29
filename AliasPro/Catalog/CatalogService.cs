using AliasPro.API.Catalog;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Catalog.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Catalog
{
    internal class CatalogService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<CatalogDao>();
            collection.AddSingleton<CatalogRepostiory>();
            collection.AddSingleton<ICatalogController, CatalogController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, RequestDiscountEvent>();
            collection.AddSingleton<IAsyncPacket, RequestCatalogIndexEvent>();
            collection.AddSingleton<IAsyncPacket, RequestCatalogModeEvent>();
            collection.AddSingleton<IAsyncPacket, RequestCatalogPageEvent>();
            collection.AddSingleton<IAsyncPacket, CatalogBuyItemEvent>();
            collection.AddSingleton<IAsyncPacket, CatalogBuyGiftItemEvent>();
            collection.AddSingleton<IAsyncPacket, RequestGiftConfigurationEvent>();
        }
    }
}
