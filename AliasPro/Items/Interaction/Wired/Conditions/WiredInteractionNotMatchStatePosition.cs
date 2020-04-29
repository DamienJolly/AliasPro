using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotMatchStatePosition : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_MATCH_SSHOT;

        public WiredInteractionNotMatchStatePosition(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item))
                    continue;

                bool stateMatches = !State || item.ExtraData == itemData.ExtraData + "";
                bool positionMatches = !Position || (item.Position.X == itemData.Position.X && item.Position.Y == itemData.Position.Y);
                bool directionMatches = !Direction || item.Rotation == itemData.Rotation;

                if (stateMatches && positionMatches && directionMatches)
                    return false;
            }

            return true;
        }

        private bool State =>
            (WiredData.Params.Count <= 0) ? false : WiredData.Params[0] == 1;

        private bool Direction =>
            (WiredData.Params.Count <= 1) ? false : WiredData.Params[1] == 1;

        private bool Position =>
            (WiredData.Params.Count <= 2) ? false : WiredData.Params[2] == 1;
    }
}
