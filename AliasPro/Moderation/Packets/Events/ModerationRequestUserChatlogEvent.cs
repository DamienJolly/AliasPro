﻿using AliasPro.API.Chat;
using AliasPro.API.Chat.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestUserChatlogEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationRequestUserChatlogMessageEvent;

		private readonly IPlayerController _playerController;
		private readonly IChatController _chatController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestUserChatlogEvent(
			IPlayerController playerController,
			IChatController chatController,
			IPermissionsController permissionsController)
		{
			_playerController = playerController;
			_chatController = chatController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_logs"))
				return;

			int playerId = message.ReadInt();

            IPlayerData player = await _playerController.GetPlayerAsync((uint)playerId);
            if (player == null)
                return;

            IDictionary<IPlayerRoomVisited, ICollection<IChatLog>> chatlogs = 
                new Dictionary<IPlayerRoomVisited, ICollection<IChatLog>>();

            foreach (IPlayerRoomVisited visit in
                await _playerController.GetPlayerRoomVisitsAsync(player.Id))
            {
                if (!chatlogs.TryAdd(visit, new List<IChatLog>()))
                    continue;

                chatlogs[visit] = await _chatController.ReadRoomChatlogs(
                    (uint)visit.RoomId, 
                    visit.EntryTimestamp, 
                    visit.ExitTimestamp);
            }
            
            await session.SendPacketAsync(new ModeratorUserChatlogComposer(player, chatlogs));
        }
    }
}