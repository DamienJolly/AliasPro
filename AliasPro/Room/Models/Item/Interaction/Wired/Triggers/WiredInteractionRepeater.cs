using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionRepeater : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.PERIODICALLY;

        private bool _active = false;
        private int _tick = 0;

        private WiredData _wiredData;

        public WiredInteractionRepeater(IItem item)
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
            message.WriteInt(1);
            message.WriteInt(_wiredData.Timer);
            message.WriteInt(0);
            message.WriteInt((int)_type);
            message.WriteInt(0);
        }

        public void SaveData(IClientPacket clientPacket)
        {
            clientPacket.ReadInt();

            int timer = clientPacket.ReadInt();

            if (timer < 1) timer = 1;
            if (timer > 120) timer = 120;

            _wiredData.Timer = timer;
            _item.ExtraData = 
                _wiredData.DataToString;
        }

        public void OnTrigger(BaseEntity entity)
        {
            if(!_active)
            {
                _active = true;
                _tick = _wiredData.Timer;
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
                        effect.WiredInteraction.OnTrigger(null);
                    }
                    _active = false;
                }
            }
        }
    }
}
