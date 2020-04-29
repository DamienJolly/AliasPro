using AliasPro.API.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionResetTimers : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.RESET_TIMERS;

        public WiredInteractionResetTimers(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (IItem item in Room.Items.GetItemsByType(ItemInteractionType.WIRED_TRIGGER))
            {
                if (item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER ||
                    item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER_LONG ||
                    item.ItemData.WiredInteractionType == WiredInteractionType.AT_GIVEN_TIME)
                {
                    item.WiredInteraction.ResetTimers();
                }
            }
            return true;
        }
    }
}
