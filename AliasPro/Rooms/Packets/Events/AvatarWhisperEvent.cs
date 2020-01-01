using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Types;

namespace AliasPro.Rooms.Packets.Events
{
    public class AvatarWhisperEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AvatarWhisperMessageEvent;

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            string text = clientPacket.ReadString();
            int colour = clientPacket.ReadInt();

            session.Entity.Unidle();

            room.OnChat(text, colour, session.Entity, RoomChatType.WHISPER);
        }
    }
}
