using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionTriggerStacks : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.CALL_STACKS;

        public WiredInteractionTriggerStacks(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            IList<IRoomTile> stacks = new List<IRoomTile>();

            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) 
                    continue;

                IList<IRoomTile> roomTiles = Room.RoomGrid.GetTilesFromItem(item.Position.X, item.Position.Y, item);

                foreach (IRoomTile roomTile in roomTiles)
                {
                    if (!stacks.Contains(roomTile))
                        stacks.Add(roomTile);
                }
            }

            foreach (IRoomTile roomTile in stacks)
            {
                Room.Items.TriggerEffects(roomTile, args);
            }
            return true;
        }
    }
}
