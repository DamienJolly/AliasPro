using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionMatchPosition : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.MATCH_POSITION;

        public WiredInteractionMatchPosition(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) 
                    continue;

                Room.RoomGrid.RemoveItem(item);

                //todo: check it state can be changed. E.g; Teleporters = false, Gates = true.
                if (ChangeState)
                    item.ExtraData = itemData.ExtraData + "";

                if (ChangeDirection)
                    item.Rotation = itemData.Rotation;

                if (ChangePosition)
                    item.Position = itemData.Position;

                Room.RoomGrid.AddItem(item);

                Room.SendPacketAsync(new FloorItemUpdateComposer(item));
            }

            return true;
        }

        private bool ChangeState =>
            (WiredData.Params.Count <= 0) ? false : WiredData.Params[0] == 1;

        private bool ChangeDirection =>
            (WiredData.Params.Count <= 1) ? false : WiredData.Params[1] == 1;

        private bool ChangePosition =>
            (WiredData.Params.Count <= 2) ? false : WiredData.Params[2] == 1;
    }
}
