using AliasPro.API.Chat;
using AliasPro.API.Chat.Commands;
using AliasPro.Chat.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Chat
{
    internal class ChatService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<ChatDao>();
            collection.AddSingleton<IChatController, ChatController>();

            RegisterCommands(collection);
        }

        private static void RegisterCommands(IServiceCollection collection)
        {
            collection.AddSingleton<IChatCommand, UpdateCommand>();
        }
    }
}
