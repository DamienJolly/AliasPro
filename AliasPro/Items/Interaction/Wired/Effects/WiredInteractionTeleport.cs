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

        public WiredInteractionTeleport(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length == 0)
                return false;

            BaseEntity target = (BaseEntity)args[0];
            IWiredItemData itemData = RandomItemData;

            if (itemData == null)
                return false;

            if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item))
                return false;

            target.SetEffect(4, 6);
            target.Room.RoomGrid.RemoveEntity(target);
            target.Position =
                target.NextPosition =
                target.GoalPosition = new RoomPosition(
                        item.Position.X,
                        item.Position.Y,
                     item.Position.Z);
            target.Room.RoomGrid.AddEntity(target);
            return true;
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
