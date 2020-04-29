using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionPlayerHasHanditem : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.ACTOR_HAS_HANDITEM;

        public WiredInteractionPlayerHasHanditem(IItem item)
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

            return target.HandItemId == HandItemId;
        }

        private int HandItemId =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];
    }
}
