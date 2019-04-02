using AliasPro.Item.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionRepeater : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.PERIODICALLY;

        private bool _active = false;
        private int _tick = 0;
        
        public WiredData WiredData { get; set; }

        public WiredInteractionRepeater(IItem item)
        {
            _item = item;
            WiredData = 
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            if(!_active)
            {
                _active = true;
                _tick = Timer;
            }
            return true;
        }

        public void OnCycle()
        {
            if (_active)
            {
                _tick--;
                if (_tick <= 0)
                {
                    if (_item.CurrentRoom.RoomMap.TryGetRoomTile(_item.Position.X, _item.Position.Y, out RoomTile roomTile))
                    {
                        _item.CurrentRoom.ItemHandler.TriggerEffects(roomTile);
                    }
                    _active = false;
                }
            }
        }

        private int Timer =>
            (WiredData.Params.Count != 1) ? 1 : WiredData.Params[0];
    }
}
