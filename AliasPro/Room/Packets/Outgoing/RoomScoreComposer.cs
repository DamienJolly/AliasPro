namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

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
