﻿using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestUserInfoEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationRequestUserInfoMessageEvent;
        
		private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestUserInfoEvent(
			IPlayerController playerController,
			IPermissionsController permissionsController)
		{
			_playerController = playerController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_player_info"))
				return;

			int playerId = message.ReadInt();

            IPlayerData player = await _playerController.GetPlayerAsync((uint)playerId);
            if (player == null)
                return;

            await session.SendPacketAsync(new ModerationUserInfoComposer(player));
        }
    }
}