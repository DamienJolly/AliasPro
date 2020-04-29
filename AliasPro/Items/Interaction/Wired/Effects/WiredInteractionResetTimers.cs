using AliasPro.API.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Utilities;

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
                switch (item.ItemData.WiredInteractionType)
                {
                    case WiredInteractionType.REPEATER:
                    case WiredInteractionType.REPEATER_LONG:
                    case WiredInteractionType.AT_GIVEN_TIME:
                        item.WiredInteraction.ResetTimers();
                        break;
                }
            }

            foreach (IItem item in Room.Items.GetItemsByType(ItemInteractionType.WIRED_EFFECT))
            {
                switch (item.WiredInteraction)
                {
                    case WiredInteractionLessTimeElapsed lessThan:
                        lessThan.LastTimerReset = (int)UnixTimestamp.Now;
                        break;
                    case WiredInteractionMoreTimeElapsed moreThan:
                        moreThan.LastTimerReset = (int)UnixTimestamp.Now;
                        break;
                }
            }
            return true;
        }
    }
}
