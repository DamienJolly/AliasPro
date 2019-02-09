namespace AliasPro.Item.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class ObjectAddComposer : IPacketComposer
    {
        private readonly IItem _item;

        public ObjectAddComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ObjectAddMessageComposer);
            _item.ComposeFloorItem(message);
            message.WriteString(_item.PlayerUsername);
            return message;
        }
    }
}
