using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Rooms.Types;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class AvatarWhisperEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.AvatarWhisperMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null)
                return Task.CompletedTask;

            string text = clientPacket.ReadString();
            int colour = clientPacket.ReadInt();

            session.Entity.Unidle();

            room.OnChat(text, colour, session.Entity, RoomChatType.WHISPER);
            return Task.CompletedTask;
        }
    }
}
