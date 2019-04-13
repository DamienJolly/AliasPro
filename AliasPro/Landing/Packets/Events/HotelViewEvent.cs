using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Room;

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
            if (session.CurrentRoom != null)
            {
                await _roomController.RemoveFromRoom(session);
            }
        }
    }
}