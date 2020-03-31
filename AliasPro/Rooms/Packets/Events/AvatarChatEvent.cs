using AliasPro.API.Chat;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Types;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class AvatarChatEvent : IMessageEvent
    {
        public short Header => Incoming.AvatarChatMessageEvent;

        private readonly IChatController _chatController;

        public AvatarChatEvent(IChatController chatController)
        {
            _chatController = chatController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null)
                return;

            string text = message.ReadString();
            int colour = message.ReadInt();

            session.Entity.Unidle();

            if (!await _chatController.HandleCommand(session, text))
            {
                room.OnChat(text, colour, session.Entity, RoomChatType.TALK);
            }
        }
    }
}
