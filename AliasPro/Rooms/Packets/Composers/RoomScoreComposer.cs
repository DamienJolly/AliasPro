using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomScoreComposer : IMessageComposer
    {
        private readonly int _score;

        public RoomScoreComposer(int score)
        {
            _score = score;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomScoreMessageComposer);
            message.WriteInt(_score);
            message.WriteBoolean(false);
            return message;
        }
    }
}
