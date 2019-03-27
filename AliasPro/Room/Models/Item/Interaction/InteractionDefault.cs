namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using AliasPro.Item.Models;
    using AliasPro.Item.Packets.Outgoing;
    using AliasPro.Room.Models.Entities;

    public class InteractionDefault : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionDefault(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(0);
            message.WriteString(_item.Mode.ToString());
        }
        
        public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }

        //todo: make object
        public async void OnUserInteract(BaseEntity entity, int state)
        {
            _item.Mode++;
            if (_item.Mode >= _item.ItemData.Modes)
            {
                _item.Mode = 0;
            }
            
            await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
        }

        public void OnCycle()
        {

        }
    }
}
