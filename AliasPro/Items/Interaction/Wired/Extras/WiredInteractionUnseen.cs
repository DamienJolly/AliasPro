using AliasPro.API.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionUnseen : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.DEFAULT;

        public WiredInteractionUnseen(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            return true;
        }
    }
}
