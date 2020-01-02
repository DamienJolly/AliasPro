using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestCreateRoomEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCreateRoomMessageEvent;

        private readonly IRoomController _roomController;
        private readonly INavigatorController _navigatorController;

        public RequestCreateRoomEvent(
            IRoomController roomController, 
            INavigatorController navigatorController)
        {
            _roomController = roomController;
            _navigatorController = navigatorController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string name = clientPacket.ReadString();
            string description = clientPacket.ReadString();
            string modelName = clientPacket.ReadString();
            int categoryId = clientPacket.ReadInt();
            int maxUsers = clientPacket.ReadInt();
            int tradeType = clientPacket.ReadInt();

            if (name.Length > 60)
                name.Substring(0, 60);

            if (name.Length <= 0) return;

            if (!_roomController.TryGetRoomModel(modelName, out _)) return;

            // todo: fix
            //if (!_navigatorController.TryGetRoomCategory((uint)categoryId, out INavigatorCategory category)) return;

            if (maxUsers > 250 || maxUsers < 10) return;

            if (tradeType > 2 || tradeType < 0) return;

            // todo: room count check

            int roomId = await _roomController.CreateRoomAsync(session.Player.Id, name, description, modelName, categoryId, maxUsers, tradeType);
            await session.SendPacketAsync(new RoomCreatedComposer(roomId, name));
        }
    }
}
