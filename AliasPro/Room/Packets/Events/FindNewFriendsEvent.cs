using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;
using System;
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
            //todo: make random util
            Random rnd = new Random();
            IRoom room = _roomController.Rooms.OrderBy(a => rnd.Next()).First();

            await session.SendPacketAsync(new ForwardToRoomComposer(room.Id));
        }
    }
}
