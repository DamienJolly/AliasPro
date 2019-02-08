using System.Threading.Tasks;

namespace AliasPro.Landing.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Room;
    using Sessions;

    public class HotelViewEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.HotelViewMessageEvent;

        private readonly IRoomController _roomController;

        public HotelViewEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task HandleAsync(
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