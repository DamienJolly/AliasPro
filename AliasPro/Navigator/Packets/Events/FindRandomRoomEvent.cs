using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Events
{
    internal class FindRandomRoomEvent : IMessageEvent
    {
        public short Header => Incoming.FindRandomRoomMessageEvent;

        private readonly IRoomController _roomController;

        public FindRandomRoomEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
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
