﻿using AliasPro.API.Permissions;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Chat;
using AliasPro.Moderation.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestRoomChatlogEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationRequestRoomChatlogMessageEvent;

        private readonly IRoomController _roomController;
        private readonly ChatController _chatController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestRoomChatlogEvent(
			IRoomController roomController,
			ChatController chatController,
			IPermissionsController permissionsController)
		{
			_roomController = roomController;
			_chatController = chatController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_room_logs"))
				return;

			message.ReadInt(); //dunno?
            int roomId = message.ReadInt();

            IRoom room = await _roomController.LoadRoom((uint)roomId);
            if (room == null)
                return;

            await session.SendPacketAsync(new ModerationRoomChatlogComposer(room, 
                await _chatController.ReadRoomChatlogs(room.Id)));
        }
    }
}