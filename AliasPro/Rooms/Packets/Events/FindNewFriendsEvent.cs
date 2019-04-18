using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Linq;

namespace AliasPro.Rooms.Packets.Events
{
    public class FindNewFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.FindNewFriendsMessageEvent;

        private readonly IRoomController _roomController;

        public FindNewFriendsEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = _roomController.Rooms.OrderBy(a => Randomness.RandomNumber()).First();
            await session.SendPacketAsync(new ForwardToRoomComposer(room.Id));
        }
    }
}
