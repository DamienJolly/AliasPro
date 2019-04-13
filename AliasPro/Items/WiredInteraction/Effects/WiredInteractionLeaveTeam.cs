using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionLeaveTeam : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.LEAVE_TEAM;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionLeaveTeam(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }
        
        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                if (args.Length == 0) return false;

                _active = true;
                _target = (BaseEntity)args[0];
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
                    if (_target != null &&
                        _target is PlayerEntity)
                    {
                        _item.CurrentRoom.Game.LeaveTeam(_target);
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
