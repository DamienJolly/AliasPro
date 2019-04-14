using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Landing.Packets.Events
{
    public class HotelViewEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.HotelViewMessageEvent;

        private readonly IRoomController _roomController;

        public HotelViewEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (session.CurrentRoom == null || 
                session.Entity == null)
                return;
            
            await session.CurrentRoom.RemoveEntity(session.Entity);
            session.Entity = null;
            session.CurrentRoom = null;
        }
    }
}