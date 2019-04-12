using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Navigator.Models;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Room;
using AliasPro.Sessions;
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

            if (!_navigatorController.TryGetCategories(category, out ICollection<INavigatorCategory> categories)) return;

            await session.SendPacketAsync(
                new NavigatorSearchResultSetComposer(session.Player.Id, category, data, categories, _roomController));
        }
    }
}
