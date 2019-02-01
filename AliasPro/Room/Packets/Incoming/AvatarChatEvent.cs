using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    public class AvatarChatEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AvatarChatMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string text = clientPacket.ReadString();
            int colour = clientPacket.ReadInt();
            session.CurrentRoom.OnChat(text, colour, session.Entity);
            return Task.CompletedTask;
        }
    }
}
