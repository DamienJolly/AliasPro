using AliasPro.API.Chat;
using AliasPro.API.Chat.Commands;
using AliasPro.API.Network;
using AliasPro.Chat.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Chat
{
    internal class ChatService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<ChatRepostiory>();
            collection.AddSingleton<IChatController, ChatController>();
            
            AddCommands(collection);
        }

        private static void AddCommands(IServiceCollection collection)
        {
            collection.AddSingleton<IChatCommand, UpdateCommand>();
        }
    }
}
