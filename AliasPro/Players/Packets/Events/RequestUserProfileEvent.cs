using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserProfileEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserProfileMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestUserProfileEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint playerId = (uint)clientPacket.ReadInt();

			IPlayerData targetPlayer = await _playerController.GetPlayerDataAsync((uint)playerId);
			if (targetPlayer == null)
				return;

            await session.SendPacketAsync(new UserProfileComposer(targetPlayer));
        }
    }
}
