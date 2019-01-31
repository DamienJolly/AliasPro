namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomScoreComposer : ServerPacket
    {
        public RoomScoreComposer(int score)
            : base(Outgoing.RoomScoreMessageComposer)
        {
            WriteInt(score);
            WriteBoolean(false);
        }
    }
}
