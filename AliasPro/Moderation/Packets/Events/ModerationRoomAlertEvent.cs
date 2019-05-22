using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Permissions;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRoomAlertEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRoomAlertMessageEvent;
        
		private readonly IRoomController _roomController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRoomAlertEvent(
			IRoomController roomController,
			IPermissionsController permissionsController)
		{
			_roomController = roomController;
			_permissionsController = permissionsController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_room_info"))
				return;

			if (session.CurrentRoom == null)
                return;

            clientPacket.ReadInt();
            string message = clientPacket.ReadString();

            await session.CurrentRoom.SendAsync(new ModerationIssueHandledComposer(message));
        }
    }
}