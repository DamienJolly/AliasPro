namespace AliasPro.Item.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RemoveWallItemComposer : IPacketComposer
    {
        private readonly IItem _item;

        public RemoveWallItemComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RemoveWallItemMessageComposer);
            message.WriteString(_item.Id + "");
            message.WriteInt(_item.PlayerId);
            return message;
        }
    }
}
