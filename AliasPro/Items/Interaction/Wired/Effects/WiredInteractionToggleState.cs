using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionToggleState : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.TOGGLE_STATE;

        public WiredInteractionToggleState(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) 
                    continue;

                item.Interaction.OnUserInteract(null);
            }
            return true;
        }
    }
}
