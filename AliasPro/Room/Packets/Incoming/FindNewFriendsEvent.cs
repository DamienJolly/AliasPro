using System;
using System.Threading.Tasks;
using System.Linq;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using Room;
    using Room.Models;

    public class FindNewFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.FindNewFriendsMessageEvent;

        private readonly IRoomController _roomController;

        public FindNewFriendsEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: make random util
            Random rnd = new Random();
            IRoom room = _roomController.GetAllRooms().OrderBy(a => rnd.Next()).First();

            await session.SendPacketAsync(new ForwardToRoomComposer(room.RoomData.Id));
        }
    }
}
