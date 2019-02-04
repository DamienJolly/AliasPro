using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Item
{
    using Network;
    using Network.Events;
    using Packets.Incoming;

    internal class ItemService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<ItemDao>();
            collection.AddSingleton<ItemRepository>();
            collection.AddSingleton<IItemController, ItemController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, PlaceItemEvent>();
            collection.AddSingleton<IAsyncPacket, UpdateItemEvent>();
            collection.AddSingleton<IAsyncPacket, RemoveItemEvent>();
        }
    }
}