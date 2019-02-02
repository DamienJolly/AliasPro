namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class FurnitureAliasesComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FurnitureAliasesMessageComposer);
            message.WriteInt(0);
            return message;
        }
    }
}
