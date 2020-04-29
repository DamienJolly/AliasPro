using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionFurniHaveFurni : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.FURNI_HAS_FURNI;

        public WiredInteractionFurniHaveFurni(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            bool foundSomething = false;
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                bool found = false;
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item))
                    continue;

                if (!Room.RoomGrid.TryGetRoomTile(item.Position.X, item.Position.Y, out IRoomTile roomTile))
                    continue;

                foreach (IItem roomItem in roomTile.Items)
                {
                    if (roomItem.Id == item.Id)
                        continue;

                    if (roomItem.Position.Z >= item.Position.Z)
                    {
                        found = true;
                        foundSomething = true;
                    }
                }

                if (AllItems)
                {
                    if (!found)
                        return false;
                }
                else
                {
                    if (found)
                        return true;
                }
            }

            return WiredData.Items.Count == 0 || foundSomething;
        }

        private bool AllItems =>
            (WiredData.Params.Count != 1) ? false : WiredData.Params[0] == 1;
    }
}
