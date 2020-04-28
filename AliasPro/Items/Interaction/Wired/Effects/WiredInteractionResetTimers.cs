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
            //todo: code this Damien you idiot!
            return true;
        }
    }
}
