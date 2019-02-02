namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class NavigatorPreferencesComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorPreferencesMessageComposer);
            message.WriteInt(68);
            message.WriteInt(42);
            message.WriteInt(425);
            message.WriteInt(592);
            message.WriteBoolean(false);
            message.WriteInt(0);
            return message;
        }
    }
}
