using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class FindNewFriendsEvent : IMessageEvent
    {
        public short Header => Incoming.FindNewFriendsMessageEvent;

        private readonly IRoomController _roomController;

        public FindNewFriendsEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = _roomController.Rooms.OrderBy(a => Randomness.RandomNumber()).First();
            await session.SendPacketAsync(new ForwardToRoomComposer(room.Id));
        }
    }
}
