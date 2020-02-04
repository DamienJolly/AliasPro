using AliasPro.API.Navigator;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestCreateRoomEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestCreateRoomMessageEvent;

        private readonly IRoomController _roomController;
        private readonly INavigatorController _navigatorController;

        public RequestCreateRoomEvent(
            IRoomController roomController, 
            INavigatorController navigatorController)
        {
            _roomController = roomController;
            _navigatorController = navigatorController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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
