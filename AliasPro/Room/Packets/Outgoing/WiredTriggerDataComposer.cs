namespace AliasPro.Room.Packets.Outgoing
{
    using AliasPro.Item.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class WiredTriggerDataComposer : IPacketComposer
    {
        private readonly IItem _item;

        public WiredTriggerDataComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.WiredTriggerDataMessageComposer);
            message.WriteBoolean(false);
            message.WriteInt(5);
            _item.WiredInteraction.Compose(message);
            return message;
        }
    }
}
