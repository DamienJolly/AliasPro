using AliasPro.API.Navigator;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Navigator.Packets.Events
{
    internal class RequestNavigatorSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestNavigatorSettingsMessageEvent;

        private readonly INavigatorController _navigatorController;

        public RequestNavigatorSettingsEvent(INavigatorController navigatorController)
        {
            _navigatorController = navigatorController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new NavigatorEventCategoriesComposer(_navigatorController.TryGetCategoryByView("roomads_view")));
        }
    }
}
