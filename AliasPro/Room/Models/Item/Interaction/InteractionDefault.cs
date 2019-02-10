namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using AliasPro.Item.Models;
    using Sessions;
    using AliasPro.Item.Packets.Outgoing;

    public class InteractionDefault : IItemInteractor
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
            item.Mode++;
            if (item.Mode >= item.ItemData.Modes)
            {
                item.Mode = 0;
            }
        }

        public void OnCycle(IItem item)
        {

        }
    }
}
