using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionVendingMachine : ItemInteraction
    {
        private int _tickCount = 0;

        public InteractionVendingMachine(IItem item)
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }

        public override void OnPlaceItem()
		{
            Item.ExtraData = "0";
        }

		public override void OnMoveItem()
		{
            Item.ExtraData = "0";
		}

        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            if (entity == null) return;

            if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
                return;

            IRoomPosition position = tile.PositionInFront(Item.Rotation);
            if (!(position.X == entity.Position.X && position.Y == entity.Position.Y))
            {
                entity.GoalPosition = position;
                return;
            }

            if (Item.ExtraData == "1") return;

            if (!entity.Actions.HasStatus("sit") &&
                !entity.Actions.HasStatus("lay"))
            {
                entity.Actions.RemoveStatus("mv");
                entity.SetRotation(entity.Position.CalculateDirection(Item.Position.X, Item.Position.Y));
            }

            Item.ExtraData = "1";
            Item.InteractingPlayer = entity;
            _tickCount = 0;

            await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
        }

        public async override void OnCycle()
        {
            if (Item.ExtraData != "1") return;

            if (_tickCount < 1)
            {
                _tickCount++;
                return;
            }

            Item.ExtraData = "0";
            int handItemId =
                GetRandomVendingMachineId(Item.ItemData.ExtraData);
            Item.InteractingPlayer.SetHandItem(handItemId);

            await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
            await Item.CurrentRoom.SendPacketAsync(new UserHandItemComposer(
                Item.InteractingPlayer.Id,
                Item.InteractingPlayer.HandItemId));
        }

        private int GetRandomVendingMachineId(string extraData)
        {
            int id = 0;
            IList<int> items = new List<int>();
            foreach (string item in extraData.Split(','))
            {
                if (int.TryParse(item, out int itemId))
                    items.Add(itemId);
            }

            if (items.Count != 0)
                id = items[Randomness.RandomNumber(items.Count) - 1];
            return id;
        }
    }
}
