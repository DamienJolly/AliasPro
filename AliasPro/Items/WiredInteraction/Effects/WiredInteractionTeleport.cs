using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionTeleport : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.TELEPORT;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

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
                        IWiredItemData itemData = RandomItemData;

                        if (itemData != null)
                        {
                            if (_item.CurrentRoom.Items.TryGetItem(itemData.ItemId, out IItem item))
                            {
                                //todo: effect
                                _item.CurrentRoom.RoomGrid.RemoveEntity(_target);
                                _target.NextPosition =
                                    new RoomPosition(item.Position.X, item.Position.Y, item.Position.Z);
                                _item.CurrentRoom.RoomGrid.AddEntity(_target);
                            }
                        }
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private IWiredItemData RandomItemData
        {
            get
            {
                IWiredItemData itemData = null;
                IList<IWiredItemData> keyList = new List<IWiredItemData>(WiredData.Items.Values);

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
