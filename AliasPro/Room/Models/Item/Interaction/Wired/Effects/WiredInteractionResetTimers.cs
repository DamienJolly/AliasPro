using AliasPro.Item.Models;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionResetTimers : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.RESET_TIMERS;

        private bool _active = false;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionResetTimers(IItem item)
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
                    _item.CurrentRoom.GameHandler.GameStarted = false;
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
