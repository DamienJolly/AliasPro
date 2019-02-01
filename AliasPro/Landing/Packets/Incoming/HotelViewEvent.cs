using System.Threading.Tasks;

namespace AliasPro.Landing.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Room.Models;
    using Sessions;

    public class HotelViewEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.HotelViewMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room != null)
            {
                room.LeaveRoom(session);
            }

            return Task.CompletedTask;
        }
    }
}