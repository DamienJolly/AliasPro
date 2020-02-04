using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestPromotionRoomsEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestPromotionRoomsMessageEvent;

        private readonly IRoomController _roomController;

        public RequestPromotionRoomsEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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
