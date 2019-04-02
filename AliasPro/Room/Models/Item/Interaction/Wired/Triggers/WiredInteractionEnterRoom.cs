﻿using AliasPro.Item.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionEnterRoom : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.ENTER_ROOM;
        
        public WiredData WiredData { get; set; }

        public WiredInteractionEnterRoom(IItem item)
        {
            _item = item;
            WiredData = 
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            if (args.Length != 1) return;

            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return;

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
