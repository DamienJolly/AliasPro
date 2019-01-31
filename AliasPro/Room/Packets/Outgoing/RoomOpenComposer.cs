namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomOpenComposer : ServerPacket
    {
        public RoomOpenComposer()
            : base(Outgoing.RoomOpenMessageComposer)
        {
        }
    }
}
