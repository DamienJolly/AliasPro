using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomScoreComposer : IPacketComposer
    {
        private readonly int _score;

        public RoomScoreComposer(int score)
        {
            _score = score;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomScoreMessageComposer);
            message.WriteInt(_score);
            message.WriteBoolean(false);
            return message;
        }
    }
}
