using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Room.Models;
    using Outgoing;

    public class UserStartTypingEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserStartTypingMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            await room.SendAsync(new UserTypingComposer(session.Entity.Id, true));
        }
    }
}
