using AliasPro.API.Crafting;
using AliasPro.Communication.Messages;
using AliasPro.Crafting.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Crafting
{
    internal class CraftingService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<CraftingDao>();
            collection.AddSingleton<ICraftingController, CraftingController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, RequestCraftingRecipesEvent>();
            collection.AddSingleton<IMessageEvent, CraftingAddRecipeEvent>();
            collection.AddSingleton<IMessageEvent, RequestCraftingRecipesAvailableEvent>();
            collection.AddSingleton<IMessageEvent, CraftingCraftItemEvent>();
            collection.AddSingleton<IMessageEvent, CraftingCraftSecretEvent>();
        }
    }
}
