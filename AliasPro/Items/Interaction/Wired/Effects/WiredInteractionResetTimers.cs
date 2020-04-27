using AliasPro.API.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionResetTimers : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.RESET_TIMERS;

        private bool _active = false;
        private int _tick = 0;

        public WiredInteractionResetTimers(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;
            }
            return true;
        }

        public override void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    //todo:
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
