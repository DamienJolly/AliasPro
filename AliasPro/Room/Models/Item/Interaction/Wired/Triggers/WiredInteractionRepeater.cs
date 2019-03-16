using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionRepeater : IWiredInteractor
    {
        private readonly IItem _item;

        private bool _active;
        private ISession _targetSession;
        private int _tick = 0;

        public WiredInteractionRepeater(IItem item)
        {
            _item = item;

            //todo: get data from database
        }

        public void Compose(ServerPacket message)
        {

        }

        public void OnTrigger(ISession session)
        {
            if (!_active)
            {
                _tick = 5;
                _targetSession = session;
                _active = true;
            }
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    foreach(IItem effect in _item.CurrentRoom.RoomMap.GetRoomTile(_item.Position.X, _item.Position.Y).WiredEffects)
                    {
                        effect.Interaction.OnUserInteract(_targetSession);
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
