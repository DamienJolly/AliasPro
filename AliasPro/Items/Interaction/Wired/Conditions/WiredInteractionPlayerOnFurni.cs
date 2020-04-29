using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionPlayerOnFurni : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.TRIGGER_ON_FURNI;

        public WiredInteractionPlayerOnFurni(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length == 0)
                return false;

            BaseEntity target = (BaseEntity)args[0];
            if (target == null)
                return false;

            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item))
                    continue;

                if (!Room.RoomGrid.TryGetRoomTile(item.Position.X, item.Position.Y, out IRoomTile roomTile))
                    continue;

                if (roomTile.Entities.Contains(target))
                    return true;
            }

            return false;
        }
    }
}
