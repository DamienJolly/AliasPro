namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class NavigatorPreferencesComposer : ServerPacket
    {
        public NavigatorPreferencesComposer()
            : base(Outgoing.NavigatorPreferencesMessageComposer)
        {
            WriteInt(68);
            WriteInt(42);
            WriteInt(425);
            WriteInt(592);
            WriteBoolean(false);
            WriteInt(0);
        }
    }
}
