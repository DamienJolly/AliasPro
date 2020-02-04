using AliasPro.API.Catalog;
using AliasPro.Catalog.Packets.Events;
using AliasPro.Communication.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Catalog
{
    internal class CatalogService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<CatalogDao>();
            collection.AddSingleton<ICatalogController, CatalogController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, RequestDiscountEvent>();
            collection.AddSingleton<IMessageEvent, RequestCatalogIndexEvent>();
            collection.AddSingleton<IMessageEvent, RequestCatalogModeEvent>();
            collection.AddSingleton<IMessageEvent, RequestCatalogPageEvent>();
            collection.AddSingleton<IMessageEvent, CatalogBuyItemEvent>();
            collection.AddSingleton<IMessageEvent, CatalogBuyGiftItemEvent>();
            collection.AddSingleton<IMessageEvent, RequestGiftConfigurationEvent>();
            collection.AddSingleton<IMessageEvent, RequestRecyclerLogicEvent>();
            collection.AddSingleton<IMessageEvent, ReloadRecyclerEvent>();
            collection.AddSingleton<IMessageEvent, RecycleEvent>();
        }
    }
}
