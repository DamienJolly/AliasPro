using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Models;

namespace AliasPro.Rooms.Packets.Composers
{
    public class WiredConditionDataComposer : IMessageComposer
    {
        private readonly IItem _item;
        private readonly IWiredData _wiredData;

        public WiredConditionDataComposer(IItem item)
        {
            _item = item;
            _wiredData =
                item.WiredInteraction.WiredData;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.WiredConditionDataMessageComposer);
            message.WriteBoolean(false);
            message.WriteInt(5);

            message.WriteInt(_wiredData.Items.Count);
            foreach (WiredItemData itemData in _wiredData.Items.Values)
                message.WriteInt((int)itemData.ItemId);

            message.WriteInt((int)_item.ItemData.SpriteId);
            message.WriteInt((int)_item.Id);
            message.WriteString(_wiredData.Message);

            message.WriteInt(_wiredData.Params.Count);
            foreach (int paramId in _wiredData.Params)
                message.WriteInt(paramId);

            message.WriteInt(0);
            message.WriteInt(_wiredData.WiredId);
            return message;
        }
    }
}
