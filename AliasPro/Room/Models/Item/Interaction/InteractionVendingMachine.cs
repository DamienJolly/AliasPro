using System.Collections.Generic;

namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using AliasPro.Item.Models;
    using Sessions;
    using AliasPro.Item.Packets.Outgoing;
    using Packets.Outgoing;
    using Utilities;

    public class InteractionVendingMachine : IItemInteractor
    {
        private int _tickCount = 0;

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

        public async void OnUserInteract(ISession session, IRoom room, IItem item, int state)
        {
            if (!room.RoomMap.TilesAdjecent(item.Position, session.Entity.Position))
            {
                //todo: walk to item
                return;
            }

            if (item.Mode == 1) return;

            if (!session.Entity.Actions.HasStatus("sit") &&
                !session.Entity.Actions.HasStatus("lay"))
            {
                session.Entity.Actions.RemoveStatus("mv");
                session.Entity.Position.CalculateDirection(item.Position);
            }

            item.Mode = 1;
            item.InteractingPlayer = session.Entity;
            _tickCount = 0;

            await room.SendAsync(new FloorItemUpdateComposer(item));
        }

        public async void OnCycle(IRoom room, IItem item)
        {
            if (item.Mode != 1) return;

            if (_tickCount < 1)
            {
                _tickCount++;
                return;
            }

            item.Mode = 0;
            int handItemId = 
                GetRandomVendingMachineId(item.ExtraData);
            item.InteractingPlayer.SetHandItem(handItemId);

            await room.SendAsync(new FloorItemUpdateComposer(item));
            await room.SendAsync(new UserHandItemComposer(
                item.InteractingPlayer.Id,
                item.InteractingPlayer.HandItemId));
        }

        private int GetRandomVendingMachineId(string extraData)
        {
            int id = 0;
            IList<int> items = new List<int>();
            foreach(string item in extraData.Split(','))
                items.Add(int.Parse(item));

            if (items.Count != 0)
                id = items[Randomness.RandomNumber(items.Count) - 1];
            return id;
        }
    }
}
