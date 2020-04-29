using AliasPro.API.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionLessTimeElapsed : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.TIME_LESS_THAN;

        public int LastTimerReset = 0;

        public WiredInteractionLessTimeElapsed(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            return ((int)UnixTimestamp.Now - LastTimerReset) / 0.5 < Cycles;
        }

        private int Cycles =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];
    }
}
