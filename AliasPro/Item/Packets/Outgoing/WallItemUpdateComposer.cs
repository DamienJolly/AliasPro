namespace AliasPro.Item.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class WallItemUpdateComposer : IPacketComposer
    {
        private readonly IItem _item;

        public WallItemUpdateComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.WallItemUpdateMessageComposer);
            _item.ComposeWallItem(message);
            message.WriteString(_item.PlayerUsername);
            return message;
        }
    }
}
