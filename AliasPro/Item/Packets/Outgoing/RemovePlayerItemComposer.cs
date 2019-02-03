namespace AliasPro.Item.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RemovePlayerItemComposer : IPacketComposer
    {
        private readonly uint _itemId;

        public RemovePlayerItemComposer(uint itemId)
        {
            _itemId = itemId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RemovePlayerItemMessageComposer);
            message.WriteInt(_itemId);
            return message;
        }
    }
}
