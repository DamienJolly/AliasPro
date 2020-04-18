﻿using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction
{
    public class InteractionDice : IItemInteractor
    {
        private readonly IItem _item;

        private int _tickCount = 0;

        public InteractionDice(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(_item.ExtraData);
        }

		public void OnPlaceItem()
		{
			_item.ExtraData = "0";
		}

		public void OnPickupItem()
		{
			_item.ExtraData = "0";
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
			if (entity == null) return;

			if (_item.ExtraData == "-1") return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			if (!tile.TilesAdjecent(entity.Position))
			{
				IRoomPosition position = tile.PositionInFront(_item.Rotation);
				entity.GoalPosition = position;
				return;
			}

			_item.ExtraData = state + "";
			_tickCount = 0;
			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
		}

        public async void OnCycle()
        {
            if (_item.ExtraData == "-1")
            {
                if (_tickCount >= 2)
                {
					_item.ExtraData = Randomness.RandomNumber(1, 6) + "";
                    await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
                }
                _tickCount++;
            }
        }
    }
}
