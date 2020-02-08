using AliasPro.API.Permissions;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRoomAlertEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationRoomAlertMessageEvent;
        
		private readonly IRoomController _roomController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRoomAlertEvent(
			IRoomController roomController,
			IPermissionsController permissionsController)
		{
			_roomController = roomController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_room_info"))
				return;

			if (session.CurrentRoom == null)
                return;

            clientPacket.ReadInt();
            string message = clientPacket.ReadString();

            await session.CurrentRoom.SendPacketAsync(new ModerationIssueHandledComposer(message));
        }
    }
}