using AliasPro.API.Chat;
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
    public class ModerationRequestRoomChatlogEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.ModerationRequestRoomChatlogMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IChatController _chatController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestRoomChatlogEvent(
			IRoomController roomController,
			IChatController chatController,
			IPermissionsController permissionsController)
		{
			_roomController = roomController;
			_chatController = chatController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_room_logs"))
				return;

			clientPacket.ReadInt(); //dunno?
            int roomId = clientPacket.ReadInt();

            IRoom room = await _roomController.LoadRoom((uint)roomId);
            if (room == null)
                return;

            await session.SendPacketAsync(new ModerationRoomChatlogComposer(room, 
                await _chatController.ReadRoomChatlogs(room.Id)));
        }
    }
}