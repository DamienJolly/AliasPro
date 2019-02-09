namespace AliasPro.Item.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class AddWallItemComposer : IPacketComposer
    {
        private readonly IItem _item;

        public AddWallItemComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddWallItemMessageComposer);
            _item.ComposeWallItem(message);
            message.WriteString(_item.PlayerUsername);
            return message;
        }
    }
}
