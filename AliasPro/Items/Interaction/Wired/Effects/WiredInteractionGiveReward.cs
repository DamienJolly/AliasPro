using AliasPro.API.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionGiveReward : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.GIVE_REWARD;

        public WiredInteractionGiveReward(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            //todo: super wired
            return true;
        }
    }
}
