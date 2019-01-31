﻿using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;
    using Packets.Outgoing;
    using Sessions;

    public class SecureLoginEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SecureLoginMessageEvent;

        private readonly IPlayerController _playerController;

        public SecureLoginEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string ssoTicket = clientPacket.ReadString();
            IPlayer player = await _playerController.GetPlayerBySsoAsync(ssoTicket);
            if (player != null)
            {
                session.Player = player;

                await session.WriteAndFlushAsync(new SecureLoginOKComposer());
                await session.WriteAndFlushAsync(new HomeRoomComposer(1));

                await session.WriteAndFlushAsync(new UserRightsComposer(session.Player));
                await session.WriteAndFlushAsync(new AvailabilityStatusComposer());
            }
        }
    }
}
