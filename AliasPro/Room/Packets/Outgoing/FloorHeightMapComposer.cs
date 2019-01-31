namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class FloorHeightMapComposer : ServerPacket
    {
        public FloorHeightMapComposer(int wallHeight, string map)
            : base(Outgoing.FloorHeightMapMessageComposer)
        {
            WriteBoolean(false);
            WriteInt(wallHeight); //todo: wall height
            WriteString(map);
        }
    }
}
