using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionToggleState : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.TOGGLE_STATE;

        private bool _active = false;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionToggleState(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }
        
        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;
            }
            return true;
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    foreach (WiredItemData itemData in WiredData.Items.Values)
                    {
                        if (!_item.CurrentRoom.Items.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        item.Interaction.OnUserInteract(null);
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
