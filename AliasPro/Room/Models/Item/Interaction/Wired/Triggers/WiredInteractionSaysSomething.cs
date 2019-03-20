using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionSaysSomething : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.SAY_COMMAND;
        
        private WiredData _wiredData;

        public WiredInteractionSaysSomething(IItem item)
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
            message.WriteInt(0);
            message.WriteInt(0);
            message.WriteInt((int)_type);
            message.WriteInt(0);
        }

        public void SaveData(IClientPacket clientPacket)
        {
            clientPacket.ReadInt();
            _wiredData.OwnerOnly = clientPacket.ReadInt() == 1;
            _wiredData.Message = clientPacket.ReadString();

            _item.ExtraData =
                _wiredData.DataToString;
        }

        public void OnTrigger(BaseEntity entity)
        {
            if (entity is UserEntity userEntity)
            {
                if (_wiredData.OwnerOnly &&
                    !_item.CurrentRoom.RightHandler.IsOwner(userEntity.Player.Id)) return;
            }

            foreach (IItem effect in _item.CurrentRoom.RoomMap.GetRoomTile(_item.Position.X, _item.Position.Y).WiredEffects)
            {
                effect.WiredInteraction.OnTrigger(entity);
            }
        }

        public void OnCycle()
        {

        }

        public bool HasItem(uint itemId) =>
           _wiredData.Items.Contains(itemId);
    }
}
