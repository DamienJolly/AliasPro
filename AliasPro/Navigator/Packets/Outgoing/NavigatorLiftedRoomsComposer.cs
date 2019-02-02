namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class NavigatorLiftedRoomsComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorLiftedRoomsMessageComposer);
            message.WriteInt(0);
            return message;
        }
    }
}
