using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Item
{
    using Network;

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

        }
    }
}