namespace AliasPro.Room.Packets.Outgoing
{
    using AliasPro.Item.Models;
    using AliasPro.Room.Models.Item.Interaction.Wired;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class WiredTriggerDataComposer : IPacketComposer
    {
        private readonly IItem _item;
        private readonly WiredData _wiredData;

        public WiredTriggerDataComposer(IItem item)
        {
            _item = item;
            _wiredData = 
                item.WiredInteraction.WiredData;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.WiredTriggerDataMessageComposer);
            message.WriteBoolean(false);
            message.WriteInt(5);

            message.WriteInt(_wiredData.Items.Count);
            foreach (WiredItemData itemData in _wiredData.Items.Values)
                message.WriteInt(itemData.ItemId);

            message.WriteInt(_item.ItemData.SpriteId);
            message.WriteInt(_item.Id);
            message.WriteString(_wiredData.Message);

            message.WriteInt(_wiredData.Params.Count);
            foreach (int paramId in _wiredData.Params)
                message.WriteInt(paramId);

            message.WriteInt(0);
            message.WriteInt(_wiredData.WiredId);
            message.WriteInt(0);
            return message;
        }
    }
}
