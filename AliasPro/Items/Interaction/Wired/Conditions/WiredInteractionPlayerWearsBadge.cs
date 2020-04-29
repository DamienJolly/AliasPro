using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionPlayerWearsBadge : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.ACTOR_WEARS_BADGE;

        public WiredInteractionPlayerWearsBadge(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length == 0)
                return false;

            PlayerEntity target = (PlayerEntity)args[0];
            if (target == null) 
                return false;

            if (!target.Player.Badge.TryGetBadge(WiredData.Message, out IPlayerBadge badge))
                return false;

            return badge.Slot != 0;
        }
    }
}
