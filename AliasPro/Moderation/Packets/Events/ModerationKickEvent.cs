﻿using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationKickEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationKickMessageEvent;
        
        private readonly IPlayerController _playerController;

        public ModerationKickEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
                return;
            
            int playerId = clientPacket.ReadInt();
			string message = clientPacket.ReadString();

			if (!_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
				return;

            if (player.Session == null || player.Session.CurrentRoom == null)
                return;

			await player.Session.CurrentRoom.RemoveEntity(session.Entity);
			player.Session.Entity = null;
			player.Session.CurrentRoom = null;

			await player.Session.SendPacketAsync(new ModerationIssueHandledComposer(message));
        }
    }
}