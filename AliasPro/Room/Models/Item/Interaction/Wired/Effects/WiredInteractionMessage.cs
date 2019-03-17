using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;
using AliasPro.Room.Packets.Outgoing;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionMessage : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.SHOW_MESSAGE;

        private bool _active = false;
        private BaseEntity _target = null;
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
            message.WriteInt((int)_type);
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

        public void OnTrigger(BaseEntity entity)
        {
            if (!_active)
            {
                _tick = _wiredData.Timer;
                _target = entity;
                _active = true;
            }
        }

        public async void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target != null)
                    {
                        if (_target is UserEntity userEntity)
                        {
                            await userEntity.Session.SendPacketAsync(new AvatarChatComposer(
                                userEntity.Id, _wiredData.Message, 0, 34));
                        }
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
