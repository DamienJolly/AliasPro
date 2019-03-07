using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RequestUserProfileEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserProfileMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestUserProfileEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();

            if (playerId <= 0) return;

            IPlayer targetPlayer = 
                await _playerController.GetPlayerByIdAsync((uint)playerId);

            if (targetPlayer == null) return;

            await session.SendPacketAsync(new UserProfileComposer(targetPlayer));
        }
    }
}
