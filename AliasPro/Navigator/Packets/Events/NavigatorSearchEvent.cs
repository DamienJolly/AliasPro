using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Events
{
    internal class NavigatorSearchEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.NavigatorSearchMessageEvent;

        private readonly INavigatorController _navigatorController;
        private readonly IRoomController _roomController;

        public NavigatorSearchEvent(INavigatorController navigatorController, IRoomController roomController)
        {
            _navigatorController = navigatorController;
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string category = clientPacket.ReadString();
            string data = clientPacket.ReadString();

            if (!_navigatorController.TryGetCategories(category, out IDictionary<uint, INavigatorCategory> categories))
                return;

            await session.SendPacketAsync(
                new NavigatorSearchResultSetComposer(session.Player.Id, category, data, categories.Values, _roomController));
        }
    }
}
