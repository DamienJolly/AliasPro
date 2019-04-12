using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Packets.Composers;
using AliasPro.Sessions;
using System;
using System.Linq;

namespace AliasPro.Room.Packets.Events
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
            IRoom room = _roomController.GetAllRooms().OrderBy(a => rnd.Next()).First();

            await session.SendPacketAsync(new ForwardToRoomComposer(room.RoomData.Id));
        }
    }
}
