using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionWalksOn : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.WALKS_ON_FURNI;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        private WiredData _wiredData;

        public WiredInteractionWalksOn(IItem item)
        {
            _item = item;
            _wiredData = 
                new WiredData(_item);
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(_wiredData.Items.Count);
            foreach (uint itemId in _wiredData.Items)
            {
                message.WriteInt(itemId);
            }
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
            clientPacket.ReadString();

            _wiredData.Items.Clear();

            int count = clientPacket.ReadInt();

            for (int i = 0; i < count; i++)
            {
                int itemId = clientPacket.ReadInt();
                _wiredData.Items.Add((uint)itemId);
            }

            _item.ExtraData = 
                _wiredData.DataToString;
        }

        public void OnTrigger(BaseEntity entity)
        {
            if (!_active)
            {
                _active = true;
                _target = entity;
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
                        effect.WiredInteraction.OnTrigger(_target);
                    }
                    _active = false;
                }
            }
        }

        public bool HasItem(uint itemId) =>
            _wiredData.Items.Contains(itemId);
    }
}
