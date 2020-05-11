using AliasPro.Game.Chat.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Chat
{
	public class ChatService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<ChatDao>();
            collection.AddSingleton<ChatController>();

            RegisterCommands(collection);
        }

        private static void RegisterCommands(IServiceCollection collection)
        {
            collection.AddSingleton<ICommand, UpdateCommand>();
            collection.AddSingleton<ICommand, GiveCommand>();
            collection.AddSingleton<ICommand, BanCommand>();
            collection.AddSingleton<ICommand, HotelAlertCommand>();
            collection.AddSingleton<ICommand, HotelAlertLinkCommand>();
            collection.AddSingleton<ICommand, EventAlertCommand>();

            collection.AddSingleton<ICommand, PushCommand>();
            collection.AddSingleton<ICommand, PullCommand>();

            collection.AddSingleton<ICommand, CommandsCommand>();
            collection.AddSingleton<ICommand, FollowCommand>();
            collection.AddSingleton<ICommand, UnloadCommand>();
        }
    }
}
