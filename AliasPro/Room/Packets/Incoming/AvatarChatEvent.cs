using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Chat;

    public class AvatarChatEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AvatarChatMessageEvent;

        private readonly IChatController _chatController;

        public AvatarChatEvent(IChatController chatController)
        {
            _chatController = chatController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string text = clientPacket.ReadString();
            int colour = clientPacket.ReadInt();

            if(!await _chatController.HandleCommandAsync(session, text))
            {
                session.CurrentRoom.OnChat(text, colour, session.Entity);
            }
        }
    }
}
