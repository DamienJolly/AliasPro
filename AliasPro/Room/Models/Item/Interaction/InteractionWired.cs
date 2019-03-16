namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using Models.Item.Interaction.Wired;
    using AliasPro.Item.Models;
    using Sessions;

    public class InteractionWired : IItemInteractor
    {
        private readonly IItem _item;
        private readonly IWiredInteractor _wiredInteraction;

        public InteractionWired(IItem item)
        {
            _item = item;
            _wiredInteraction = 
                WiredInteractor.GetWiredInteractor(item.ItemData.WiredInteractionType, item);
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

        public void OnUserInteract(ISession session, int state)
        {
            _wiredInteraction.OnTrigger(session);
        }

        public void OnCycle()
        {
            if (_item.ItemData.WiredInteractionType == WiredInteraction.REPEATER)
            {
                _item.Interaction.OnUserInteract(null);
            }

            _wiredInteraction.OnCycle();
        }
    }
}
