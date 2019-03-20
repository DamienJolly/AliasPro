using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionGameEnds : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.GAME_ENDS;
        
        private WiredData _wiredData;

        public WiredInteractionGameEnds(IItem item)
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

        }

        public void OnTrigger(BaseEntity entity)
        {
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
