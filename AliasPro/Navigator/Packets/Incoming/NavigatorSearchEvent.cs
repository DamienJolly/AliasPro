using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Incoming
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Room;

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

        public async Task HandleAsync(
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
