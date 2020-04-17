using AliasPro.API.Items;
using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Tasks;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Interaction
{
    public class InteractionCrackable : IItemInteractor
    {
        private readonly IItem _item;

        public bool Cracked = false;

        public InteractionCrackable(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(7);

            int.TryParse(_item.ExtraData, out int hits);
            int totalHits = 0;
            int crackState = 0;

            if (Program.GetService<IItemController>().TryGetCrackableDataById((int)_item.ItemData.Id, out ICrackableData crackable))
            {
                totalHits = crackable.Count;
                crackState = crackable.CalculateCrackState(hits, _item.ItemData.Modes - 1);
            }

            message.WriteString(crackState + "");
            message.WriteInt(hits);
            message.WriteInt(totalHits);

        }

        public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{

		}

		public void OnMoveItem()
		{

		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
            if (!(entity is PlayerEntity playerEntity))
                return;

            if (Cracked)
                return;

            if (_item.PlayerId != playerEntity.Player.Id)
                return;

            if (!Program.GetService<IItemController>().TryGetCrackableDataById((int)_item.ItemData.Id, out ICrackableData crackable))
                return;

            //todo: required effects

            if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
                return;

            if (!tile.TilesAdjecent(playerEntity.Position))
            {
                IRoomPosition position = tile.PositionInFront(_item.Rotation);
                playerEntity.GoalPosition = position;
                return;
            }

            int.TryParse(_item.ExtraData, out int hits);
            hits++;
            _item.ExtraData = "" + hits;
            await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));

            //todo: progress tick achievement

            if (!Cracked && hits == crackable.Count)
            {
                Cracked = true;
                await TaskManager.ExecuteTask(new CrackableExplode(_item, playerEntity.Session, true), 1500);

                //todo: progress cracked achievement
                //todo: subscriptions (for sub boxes)
            }
        }

        public void OnCycle()
        {

        }
    }
}
