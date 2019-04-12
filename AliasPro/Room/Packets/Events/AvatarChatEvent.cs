using AliasPro.API.Chat;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Sessions;

namespace AliasPro.Room.Packets.Events
{
    public class AvatarChatEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AvatarChatMessageEvent;

        private readonly IChatController _chatController;

        public AvatarChatEvent(IChatController chatController)
        {
            _chatController = chatController;
        }

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            string text = clientPacket.ReadString();
            int colour = clientPacket.ReadInt();

            room.EntityHandler.Unidle(session.Entity);

            if (!_chatController.HandleCommand(session, text))
            {
                room.OnChat(text, colour, session.Entity);
            }
        }
    }
}
