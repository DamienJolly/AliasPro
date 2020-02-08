using AliasPro.API.Permissions;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestRoomInfoEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationRequestRoomInfoMessageEvent;

		private readonly IRoomController _roomController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestRoomInfoEvent(
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

			int roomId = clientPacket.ReadInt();

			IRoom room = await _roomController.LoadRoom((uint)roomId);
			if (room == null)
				return;

            await session.SendPacketAsync(new ModerationRoomInfoComposer(room));
        }
    }
}