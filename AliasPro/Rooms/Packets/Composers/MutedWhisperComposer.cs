using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class MutedWhisperComposer : IPacketComposer
    {
        private readonly int _seconds;

        public MutedWhisperComposer(int seconds)
        {
            _seconds = seconds;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.MutedWhisperMessageComposer);
            message.WriteInt(_seconds);
            return message;
        }
    }
}
