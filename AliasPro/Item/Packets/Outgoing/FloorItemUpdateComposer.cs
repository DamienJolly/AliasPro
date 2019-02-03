namespace AliasPro.Item.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class FloorItemUpdateComposer : IPacketComposer
    {
        private readonly IItem _item;

        public FloorItemUpdateComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FloorItemUpdateMessageComposer);
            _item.Compose(message);
            return message;
        }
    }
}
