using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;
using AliasPro.Room.Packets.Outgoing;
using AliasPro.Sessions;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionMessage : IWiredInteractor
    {
        private readonly IItem _item;

        private bool _active = false;
        private ISession _targetSession = null;
        private int _tick = 0;

        private WiredData _wiredData;

        public WiredInteractionMessage(IItem item)
        {
            _item = item;
            _wiredData =
                new WiredData(_item.ExtraData);
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(0);
            message.WriteInt(_item.ItemData.SpriteId);
            message.WriteInt(_item.Id);
            message.WriteString(_wiredData.Message);
            message.WriteInt(0);
            message.WriteInt(0);
            message.WriteInt(7);
            message.WriteInt(_wiredData.Timer);
            message.WriteInt(0);
        }

        public void SaveData(IClientPacket clientPacket)
        {
            clientPacket.ReadInt();

            _wiredData.Message = 
                clientPacket.ReadString();
            clientPacket.ReadInt();

            int timer = clientPacket.ReadInt();

            if (timer < 0) timer = 0;
            if (timer > 20) timer = 20;

            _wiredData.Timer = timer;
            _item.ExtraData =
                _wiredData.DataToString;
        }

        public void OnTrigger(ISession session)
        {
            if (!_active)
            {
                _tick = _wiredData.Timer;
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
                            _targetSession.Entity.Id, _wiredData.Message, 0, 34));
                    }
                    else
                    {
                        foreach (UserEntity entity in _item.CurrentRoom.EntityHandler.Entities)
                        {
                            await entity.Session.SendPacketAsync(new AvatarChatComposer(
                            entity.Id, _wiredData.Message, 0, 34));
                        }
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
