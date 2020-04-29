using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotFurniHavePlayer : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_FURNI_HAVE_HABBO;

        public WiredInteractionNotFurniHavePlayer(IItem item)
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

                if (roomTile.Entities.Count == 0)
                    return true;
            }

            return false;
        }
    }
}
