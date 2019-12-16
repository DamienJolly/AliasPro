using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Events
{
    internal class FindRandomRoomEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.FindRandomRoomMessageEvent;

        private readonly IRoomController _roomController;

        public FindRandomRoomEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IList<IRoomData> rooms = new List<IRoomData>();
            foreach (IRoomData roomData in _roomController.Rooms)
            {
                if (roomData.UsersNow <= 0)
                    return;

                rooms.Add(roomData);
            }

            if (rooms.Count <= 0) return;

            IRoomData room = rooms[Randomness.RandomNumber(0, rooms.Count)];
            await session.SendPacketAsync(new ForwardToRoomComposer(room.Id));
        }
    }
}
