namespace AliasPro.Item.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RemoveFloorItemComposer : IPacketComposer
    {
        private readonly IItem _item;

        public RemoveFloorItemComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RemoveFloorItemMessageComposer);
            message.WriteString(_item.Id + "");
            message.WriteBoolean(false);
            message.WriteInt(_item.PlayerId);
            message.WriteInt(0);
            return message;
        }
    }
}
