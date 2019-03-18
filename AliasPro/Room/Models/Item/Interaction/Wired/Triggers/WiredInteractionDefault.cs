using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionDefault : IWiredInteractor
    {
        private readonly IItem _item;

        public WiredInteractionDefault(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(0);
            message.WriteInt(_item.ItemData.SpriteId);
            message.WriteInt(_item.Id);
            message.WriteString(string.Empty);
            message.WriteInt(0);
            message.WriteInt(0);
            message.WriteInt(0);
            message.WriteInt(0);
        }

        public void SaveData(IClientPacket clientPacket)
        {

        }

        public void OnTrigger(BaseEntity entity)
        {

        }

        public void OnCycle()
        {

        }

        public bool HasItem(uint itemId) =>
            false;
    }
}
