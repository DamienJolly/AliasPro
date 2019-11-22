using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionLoveLock : IItemInteractor
    {
        private readonly IItem _item;

		public InteractionLoveLock(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(2);
			message.WriteInt(6);

			if (!string.IsNullOrEmpty(_item.ExtraData))
			{
				string[] data = _item.ExtraData.Split(";");

				message.WriteString("1");
				message.WriteString(data[0]);
				message.WriteString(data[1]);
				message.WriteString(data[2]);
				message.WriteString(data[3]);
				message.WriteString(data[4]);
			}
			else
			{
				message.WriteString("0");
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
			}

		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
			if (!string.IsNullOrEmpty(_item.ExtraData))
				return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			//adjacent check

			if (_item.InteractingPlayer == null || !_item.CurrentRoom.Entities.HasEntity(_item.InteractingPlayer.Id))
			{
				if (_item.InteractingPlayerTwo != null && entity.Id == _item.InteractingPlayerTwo.Id)
					return;

				_item.InteractingPlayer = entity;

				if (entity is PlayerEntity playerEntity)
					await playerEntity.Player.Session.SendPacketAsync(new LoveLockStartComposer(_item));
			}
			else
			{
				if (_item.InteractingPlayer != null && entity.Id == _item.InteractingPlayer.Id)
					return;

				_item.InteractingPlayerTwo = entity;

				if (entity is PlayerEntity playerEntity)
					await playerEntity.Player.Session.SendPacketAsync(new LoveLockStartComposer(_item));
			}
        }

        public void OnCycle()
        {

        }
    }
}
