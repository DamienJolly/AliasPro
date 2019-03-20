using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionRepeater : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.PERIODICALLY;

        private bool _active = false;
        private int _tick = 0;
        
        public IWiredData WiredData { get; set; }

        public WiredInteractionRepeater(IItem item)
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
            (WiredData.Params.Count != 1) ? 10 : WiredData.Params[0];
    }
}
