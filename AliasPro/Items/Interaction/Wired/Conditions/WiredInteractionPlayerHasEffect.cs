using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionPlayerHasEffect : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.ACTOR_WEARS_EFFECT;

        public WiredInteractionPlayerHasEffect(IItem item)
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

            return target.EffectId == EffectId;
        }

        private int EffectId =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];
    }
}
