using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestCreateRoomEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCreateRoomMessageEvent;

        private readonly IRoomController _roomController;

        public RequestCreateRoomEvent(IRoomController roomController)
        {
            _roomController = roomController;
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

            // todo: room name/description checks

            if (!_roomController.TryGetRoomModel(modelName, out IRoomModel model)) return;

            // todo: category check

            if (maxUsers > 250 || maxUsers < 10) return;

            if (tradeType > 2 || tradeType < 0) return;

            // todo: room count check

            IRoomData roomData = new RoomData(
                session.Player.Id,
                session.Player.Username,
                name, 
                description, 
                modelName, 
                maxUsers, 
                tradeType, 
                categoryId);
            //todo:
           // roomData.Id = 
               // (uint)await _roomController.AddNewRoomAsync(roomData);

            await session.SendPacketAsync(new RoomCreatedComposer(roomData));
        }
    }
}
