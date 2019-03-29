using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionLeaveTeam : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.LEAVE_TEAM;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionLeaveTeam(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }
        
        public void OnTrigger(params object[] args)
        {
            if (!_active)
            {
                if (args.Length == 0) return;

                _active = true;
                _target = (BaseEntity)args[0];
                _tick = WiredData.Delay;
            }
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target != null &&
                        _target is UserEntity)
                    {
                        _item.CurrentRoom.GameHandler.LeaveTeam(_target);
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
