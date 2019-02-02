namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class AvailabilityStatusComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AvailabilityStatusMessageComposer);
            message.WriteBoolean(true);
            message.WriteBoolean(false);
            message.WriteBoolean(true);
            return message;
        }
    }
}
