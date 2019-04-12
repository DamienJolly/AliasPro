﻿using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Items.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Room.Packets.Composers
{
    public class WiredEffectDataComposer : IPacketComposer
    {
        private readonly IItem _item;
        private readonly IWiredData _wiredData;

        public WiredEffectDataComposer(IItem item)
        {
            _item = item;
            _wiredData =
                item.WiredInteraction.WiredData;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.WiredEffectDataMessageComposer);
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
            message.WriteInt(_wiredData.Delay);
            message.WriteInt(0);
            return message;
        }
    }
}
