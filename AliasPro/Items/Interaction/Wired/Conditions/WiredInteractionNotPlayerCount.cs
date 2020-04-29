using AliasPro.API.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotPlayerCount : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_USER_COUNT;

        public WiredInteractionNotPlayerCount(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            int count = Room.Entities.Entities.Count;

            return count < LowerCount || count > UpperCount;
        }

        private int LowerCount =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];

        private int UpperCount =>
            (WiredData.Params.Count <= 1) ? 1 : WiredData.Params[1];
    }
}
