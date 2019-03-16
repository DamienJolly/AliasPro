namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using AliasPro.Item.Models;
    using Sessions;
    using AliasPro.Item.Packets.Outgoing;

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

        public void OnUserEnter(ISession session)
        {

        }

        public void OnUserLeave(ISession session)
        {

        }

        public void OnUserWalkOn(ISession session)
        {

        }

        public void OnUserWalkOff(ISession session)
        {

        }

        public async void OnUserInteract(ISession session, int state)
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
