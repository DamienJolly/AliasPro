﻿using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RequestWearingBadgesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestWearingBadgesMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestWearingBadgesEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();

            IPlayer targetPlayer =
                await _playerController.GetPlayerByIdAsync((uint)playerId);

            if (targetPlayer == null || 
                targetPlayer.Badge == null) return;

            await session.SendPacketAsync(new UserBadgesComposer(targetPlayer.Badge.WearableBadges, targetPlayer.Id));
        }
    }
}