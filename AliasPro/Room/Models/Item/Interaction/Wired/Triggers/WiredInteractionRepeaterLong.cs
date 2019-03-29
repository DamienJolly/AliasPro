using AliasPro.Item.Models;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionRepeaterLong : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.PERIODICALLY_LONG;

        private bool _active = false;
        private int _tick = 0;
        
        public WiredData WiredData { get; set; }

        public WiredInteractionRepeaterLong(IItem item)
        {
            _item = item;
            WiredData = 
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            if(!_active)
            {
                _active = true;
                _tick = Timer;
            }
        }

        public void OnCycle()
        {
            if (_active)
            {
                _tick--;
                if (_tick <= 0)
                {
                    foreach (IItem effect in _item.CurrentRoom.RoomMap.GetRoomTile(_item.Position.X, _item.Position.Y).WiredEffects)
                    {
                        effect.WiredInteraction.OnTrigger();
                    }
                    _active = false;
                }
            }
        }

        private int Timer =>
            ((WiredData.Params.Count != 1) ? 1 : WiredData.Params[0]) * 10;
    }
}
