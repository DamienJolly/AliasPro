using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotPlayerWearsBadge : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_ACTOR_WEARS_BADGE;

        public WiredInteractionNotPlayerWearsBadge(IItem item)
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
                return true;

            return badge.Slot == 0;
        }
    }
}
