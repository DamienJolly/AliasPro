﻿using AliasPro.Item.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionWalksOff : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.WALKS_OFF_FURNI;

        public WiredData WiredData { get; set; }

        public WiredInteractionWalksOff(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return;

            IItem item = (IItem)args[1];
            if (item == null) return;

            if (!WiredData.Items.ContainsKey(item.Id)) return;

            if (_item.CurrentRoom.RoomMap.TryGetRoomTile(_item.Position.X, _item.Position.Y, out RoomTile roomTile))
            {
                _item.CurrentRoom.ItemHandler.TriggerEffects(roomTile, entity);
            }
        }

        public void OnCycle()
        {

        }
    }
}
