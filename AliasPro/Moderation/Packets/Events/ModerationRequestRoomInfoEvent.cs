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
    public class ModerationRequestRoomInfoEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRequestRoomInfoMessageEvent;

		private readonly IRoomController _roomController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestRoomInfoEvent(
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

			int roomId = clientPacket.ReadInt();

			IRoom room = await _roomController.LoadRoom((uint)roomId);
			if (room == null)
				return;

            await session.SendPacketAsync(new ModerationRoomInfoComposer(room));
        }
    }
}