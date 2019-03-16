using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Packets.Outgoing;
using AliasPro.Sessions;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionMessage : IWiredInteractor
    {
        private readonly IItem _item;

        private bool _active;
        private ISession _targetSession;
        private int _tick = 0;

        public WiredInteractionMessage(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {

        }

        public void OnTrigger(ISession session)
        {
            if (!_active)
            {
                _tick = 0;
                _targetSession = session;
                _active = true;
            }
        }

        public async void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_targetSession != null)
                    {
                        await _targetSession.SendPacketAsync(new AvatarChatComposer(
                            _targetSession.Entity.Id, "testing", 0, 34));
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
