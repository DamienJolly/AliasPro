using AliasPro.API.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionMoveFurniTo : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.MOVE_FURNI_TO;

        public WiredInteractionMoveFurniTo(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length <= 1)
                return false;

            IItem targetItem = (IItem)args[1];
            if (targetItem == null)
                return false;

            //todo: idk how the fuck this works
            return true;
        }
    }
}
