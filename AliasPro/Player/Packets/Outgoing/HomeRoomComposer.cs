namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class HomeRoomComposer : ServerPacket
    {
        public HomeRoomComposer(int roomId)
            : base(Outgoing.HomeRoomMessageComposer)
        {
            WriteInt(roomId);
            WriteInt(roomId);
        }
    }
}
