namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using Packets.Outgoing;
    using AliasPro.Item.Models;
    using Sessions;

    public class InteractionWired : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionWired(IItem item)
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
            if (_item.ItemData.InteractionType == ItemInteraction.WIRED_TRIGGER)
            {
                await session.SendPacketAsync(new WiredTriggerDataComposer(_item));
            }
            else if (_item.ItemData.InteractionType == ItemInteraction.WIRED_EFFECT)
            {
                await session.SendPacketAsync(new WiredEffectDataComposer(_item));
            }
        }

        public void OnCycle()
        {
            if (_item.ItemData.WiredInteractionType == WiredInteraction.REPEATER)
            {
                _item.WiredInteraction.OnTrigger(null);
            }
             
            _item.WiredInteraction.OnCycle();
        }
    }
}
