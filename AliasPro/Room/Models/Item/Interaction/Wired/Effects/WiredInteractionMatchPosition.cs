﻿using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionMatchPosition : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.MATCH_POSITION;

        private bool _active = false;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionMatchPosition(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }
        
        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;
            }
            return true;
        }

        public async void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    foreach (WiredItemData itemData in WiredData.Items.Values)
                    {
                        if (!_item.CurrentRoom.ItemHandler.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        _item.CurrentRoom.RoomMap.RemoveItem(item);
                        
                        if (ChangeState)
                            item.Mode = itemData.Mode;

                        if (ChangeDirection)
                            item.Rotation = itemData.Rotation;

                        if (ChangePosition)
                            item.Position = itemData.Position;

                        _item.CurrentRoom.RoomMap.AddItem(item);

                        await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(item));
                    }

                    _active = false;
                }
                _tick--;
            }
        }

        private bool ChangeState =>
            (WiredData.Params.Count <= 0) ? false : WiredData.Params[0] == 1;

        private bool ChangeDirection =>
            (WiredData.Params.Count <= 1) ? false : WiredData.Params[1] == 1;

        private bool ChangePosition =>
            (WiredData.Params.Count <= 2) ? false : WiredData.Params[2] == 1;
    }
}
