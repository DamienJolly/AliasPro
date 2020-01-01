using AliasPro.API.Chat;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Types;

namespace AliasPro.Rooms.Packets.Events
{
    public class AvatarShoutEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AvatarShoutMessageEvent;

        private readonly IChatController _chatController;

        public AvatarShoutEvent(IChatController chatController)
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

            session.Entity.Unidle();

            if (!_chatController.HandleCommand(session, text))
            {
                room.OnChat(text, colour, session.Entity, RoomChatType.SHOUT);
            }
        }
    }
}
