using AliasPro.Item.Models;

namespace AliasPro.Room.Models.Item.Interaction.Wired
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
        
        public void OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;
            }
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    foreach (uint itemId in WiredData.Items)
                    {
                        if (!_item.CurrentRoom.ItemHandler.TryGetItem(itemId, out IItem item)) return;

                        item.Interaction.OnUserInteract(null);
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
