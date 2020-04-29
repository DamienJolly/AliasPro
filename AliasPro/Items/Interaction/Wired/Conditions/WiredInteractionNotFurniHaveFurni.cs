using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotFurniHaveFurni : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_FURNI_HAVE_FURNI;

        public WiredInteractionNotFurniHaveFurni(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item))
                    continue;

                if (!Room.RoomGrid.TryGetRoomTile(item.Position.X, item.Position.Y, out IRoomTile roomTile))
                    continue;

                IList<IItem> itemsLeft = new List<IItem>();
                foreach (IItem roomItem in roomTile.Items)
                {
                    if (roomItem.Id == item.Id)
                        continue;

                    if (roomItem.Position.Z < item.Position.Z)
                        continue;

                    itemsLeft.Add(roomItem);
                }

                if (itemsLeft.Count != 0)
                {
                    if (AllItems)
                        return false;
                    else
                        continue;
                }
                return true;
            }

            return WiredData.Items.Count == 0;
        }

        private bool AllItems =>
            (WiredData.Params.Count != 1) ? false : WiredData.Params[0] == 1;
    }
}
