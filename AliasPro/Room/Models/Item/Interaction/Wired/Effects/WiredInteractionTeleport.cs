using AliasPro.Item.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;
using AliasPro.Utilities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionTeleport : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.TELEPORT;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionTeleport(IItem item)
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

                if (args.Length != 0)
                    _target = (BaseEntity)args[0];
            }
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target == null) return;
                    
                    uint itemId = 
                        WiredData.Items[Randomness.RandomNumber(WiredData.Items.Count) - 1];

                    if (!_item.CurrentRoom.ItemHandler.TryGetItem(itemId, out IItem item)) return;

                    //todo: effect
                    _item.CurrentRoom.RoomMap.RemoveEntity(_target);
                    _target.NextPosition = 
                        new Position(item.Position.X, item.Position.Y, item.Position.Z);
                    _item.CurrentRoom.RoomMap.AddEntity(_target);

                    _active = false;
                }
                _tick--;
            }
        }
    }
}
