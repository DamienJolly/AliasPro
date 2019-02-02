using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;
    using AliasPro.Item;

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

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int roomId = clientPacket.ReadInt();
            string password = clientPacket.ReadString();

            IRoom room = await _roomController.GetRoomByIdAndPassword(roomId, password);
            if (room != null)
            {
                await session.SendPacketAsync(new RoomOpenComposer());
                await session.SendPacketAsync(new RoomModelComposer(room.RoomModel.Id, room.RoomData.Id));
                await session.SendPacketAsync(new RoomScoreComposer(room.RoomData.Score));
                
                if (session.CurrentRoom != null)
                {
                    session.CurrentRoom.LeaveRoom(session);
                }
                session.CurrentRoom = room;
            }
            else
            {
                //todo: close connection to room
            }
        }
    }
}
