using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Items.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Items
{
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
            collection.AddSingleton<IAsyncPacket, UpdateWallEvent>();
            collection.AddSingleton<IAsyncPacket, ToggleFloorItemEvent>();
            collection.AddSingleton<IAsyncPacket, ToggleWallItemEvent>();
        }
    }
}