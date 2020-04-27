using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionToggleState : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.TOGGLE_STATE;

        private bool _active = false;
        private int _tick = 0;

        public WiredInteractionToggleState(IItem item)
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
                    foreach (WiredItemData itemData in WiredData.Items.Values)
                    {
                        if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        item.Interaction.OnUserInteract(null);
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
