namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class NavigatorLiftedRoomsComposer : ServerPacket
    {
        public NavigatorLiftedRoomsComposer()
            : base(Outgoing.NavigatorLiftedRoomsMessageComposer)
        {
            WriteInt(0);
        }
    }
}
