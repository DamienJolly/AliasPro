namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using AliasPro.Item.Models;
    using Sessions;
    using AliasPro.Item.Packets.Outgoing;

    public class InteractionWired : IItemInteractor
    {
        public void Compose(ServerPacket message, IItem item)
        {
            message.WriteInt(0);
            message.WriteString(item.Mode.ToString());
        }

        public void OnUserEnter(ISession session, IItem item)
        {

        }

        public void OnUserLeave(ISession session, IItem item)
        {

        }

        public void OnUserWalkOn(ISession session, IRoom room, IItem item)
        {

        }

        public void OnUserWalkOff(ISession session, IRoom room, IItem item)
        {

        }

        public void OnUserInteract(ISession session, IRoom room, IItem item, int state)
        {

        }

        public void OnCycle(IRoom room, IItem item)
        {
            if (item.ItemData.WiredInteractionType == WiredInteraction.REPEATER)
            {
                item.WiredInteraction.OnTrigger(null, room, item);
            }

            item.WiredInteraction.OnCycle(room, item);
        }
    }
}
