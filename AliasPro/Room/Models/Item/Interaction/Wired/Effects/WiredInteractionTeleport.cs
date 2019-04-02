using AliasPro.Item.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionTeleport : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.TELEPORT;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionTeleport(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }
        
        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                if (args.Length == 0) return false;

                _active = true;
                _target = (BaseEntity)args[0];
                _tick = WiredData.Delay;
            }
            return true;
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target != null)
                    {
                        WiredItemData itemData = RandomItemData;

                        if (itemData != null)
                        {
                            if (_item.CurrentRoom.ItemHandler.TryGetItem(itemData.ItemId, out IItem item))
                            {
                                //todo: effect
                                _item.CurrentRoom.RoomMap.RemoveEntity(_target);
                                _target.NextPosition =
                                    new Position(item.Position.X, item.Position.Y, item.Position.Z);
                                _item.CurrentRoom.RoomMap.AddEntity(_target);
                            }
                        }
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private WiredItemData RandomItemData
        {
            get
            {
                WiredItemData itemData = null;
                IList<WiredItemData> keyList = new List<WiredItemData>(WiredData.Items.Values);

                if (keyList.Count != 0)
                {
                    int randomNumber = Randomness.RandomNumber(keyList.Count) - 1;
                    itemData = keyList[randomNumber];
                }

                return itemData;
            }
        }
    }
}
