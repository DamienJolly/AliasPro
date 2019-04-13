using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Packets.Events
{
    public class RequestProfileFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestProfileFriendsMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestProfileFriendsEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint playerId = (uint)clientPacket.ReadInt();

            if (!_playerController.TryGetPlayer(playerId, out IPlayer targetPlayer))
                return;

            //todo: remove/fix
            if (targetPlayer.Messenger == null) return;

            await session.SendPacketAsync(new ProfileFriendsComposer(targetPlayer.Id, targetPlayer.Messenger.Friends));
        }
    }
}
