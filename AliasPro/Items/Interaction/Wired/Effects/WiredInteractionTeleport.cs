using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionTeleport : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.TELEPORT;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredInteractionTeleport(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
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

        public override void OnCycle()
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
                            if (Room.Items.TryGetItem(itemData.ItemId, out IItem item))
                            {
                                _target.SetEffect(4, 6);
                                _target.Room.RoomGrid.RemoveEntity(_target);
                                _target.Position = 
                                    _target.NextPosition = 
                                    _target.GoalPosition = new RoomPosition(
                                            item.Position.X,
                                            item.Position.Y,
                                         item.Position.Z);
                                _target.Room.RoomGrid.AddEntity(_target);
                                _target = null;
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
