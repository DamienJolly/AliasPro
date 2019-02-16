using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Room.Models;
    using Outgoing;

    public class UserStopTypingEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserStopTypingMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            await room.SendAsync(new UserTypingComposer(session.Entity.Id, false));
        }
    }
}
