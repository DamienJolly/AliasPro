using AliasPro.API.Messenger;
using AliasPro.API.Permissions;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationChangeRoomSettingsEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationChangeRoomSettingsMessageEvent;
        
        private readonly IRoomController _roomController;
		private readonly IPermissionsController _permissionsController;
		private readonly IMessengerController _messengerController;

		public ModerationChangeRoomSettingsEvent(
			IRoomController roomController,
			IPermissionsController permissionsController,
			IMessengerController messengerController)
		{
			_roomController = roomController;
			_permissionsController = permissionsController;
			_messengerController = messengerController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_room_settings"))
				return;

			int roomId = clientPacket.ReadInt();
			if(!_roomController.TryGetRoom((uint)roomId, out IRoom room))
                return;

            bool lockDoor = clientPacket.ReadInt() == 1;
			if (lockDoor)
				room.DoorState = 1;

			bool changeTitle = clientPacket.ReadInt() == 1;
			if (changeTitle)
				room.Name = "Inappropriate to hotel management!";

			bool kickUsers = clientPacket.ReadInt() == 1;
			if (kickUsers)
			{
				foreach (BaseEntity entity in room.Entities.Entities.ToList())
				{
					if (entity is PlayerEntity playerEntity)
					{
						await playerEntity.Session.CurrentRoom.RemoveEntity(entity);

						if (playerEntity.Player.Messenger != null)
							await _messengerController.UpdateStatusAsync(playerEntity.Player, playerEntity.Player.Messenger.Friends);
					}
				}
			}
		}
    }
}