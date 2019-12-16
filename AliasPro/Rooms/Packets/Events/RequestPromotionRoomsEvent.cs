using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestPromotionRoomsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestPromotionRoomsMessageEvent;

        private readonly IRoomController _roomController;

        public RequestPromotionRoomsEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (session.Player == null)
                return;

            ICollection<IRoomData> rooms = new List<IRoomData>();
            foreach (IRoomData room in 
                await _roomController.GetPlayersRooms(session.Player.Id))
            {
                if (room.OwnerId == session.Player.Id)
                    rooms.Add(room);
            }

            await session.SendPacketAsync(new PromoteOwnRoomsListComposer(rooms));
        }
    }
}
