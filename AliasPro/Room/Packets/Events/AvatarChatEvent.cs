using AliasPro.API.Chat;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Rooms.Packets.Events
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

            room.Entities.Unidle(session.Entity);

            if (!_chatController.HandleCommand(session, text))
            {
                room.OnChat(text, colour, session.Entity);
            }
        }
    }
}
