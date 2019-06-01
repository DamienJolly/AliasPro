using AliasPro.API.Items;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomLoadEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomLoadMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;

        public RequestRoomLoadEvent(IRoomController roomController, IItemController itemController)
        {
            _roomController = roomController;
            _itemController = itemController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            string password = clientPacket.ReadString();

            if (session.CurrentRoom != null)
				await session.CurrentRoom.RemoveEntity(session.Entity, false);

			if (!_roomController.TryGetRoom(roomId, out IRoom room))
            {
                // close connection
                return;
            }

            if (room.Password != password)
            {
                // close connection
                return;
            }

            session.CurrentRoom = room;

            await session.SendPacketAsync(new RoomOpenComposer());
            await session.SendPacketAsync(new RoomModelComposer(room.RoomModel.Id, room.Id));
            await session.SendPacketAsync(new RoomScoreComposer(room.Score));
        }
    }
}
