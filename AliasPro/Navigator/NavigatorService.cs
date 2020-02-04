using AliasPro.API.Navigator;
using AliasPro.Communication.Messages;
using AliasPro.Navigator.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Navigator
{
    internal class NavigatorService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<NavigatorDao>();
            collection.AddSingleton<INavigatorController, NavigatorController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, InitializeNavigatorEvent>();
            collection.AddSingleton<IMessageEvent, RequestUserFlatCatsEvent>();
            collection.AddSingleton<IMessageEvent, RequestNavigatorSettingsEvent>();
            collection.AddSingleton<IMessageEvent, NavigatorSearchEvent>();
            collection.AddSingleton<IMessageEvent, UpdateNavigatorPreferencesEvent>();
            collection.AddSingleton<IMessageEvent, FindRandomRoomEvent>();
        }
    }
}
