using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Player.Models;
using AliasPro.Messenger.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Players;
using AliasPro.Sessions;

namespace AliasPro.Messenger.Packets.Events
{
    public class FriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.FriendRequestMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public FriendRequestEvent(IPlayerController playerController, IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string username = clientPacket.ReadString();

            if (!_playerController.TryGetPlayer(username, out IPlayer targetPlayer))
                return;

            if (session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                return;

            if (session.Player.Messenger.TryGetRequest(targetPlayer.Id, out IMessengerRequest request))
                return;

            IMessengerRequest newRequest = new MessengerRequest(session.Player);
            await _messengerController.AddRequestAsync(targetPlayer.Id, session.Player.Id);

            if (targetPlayer.Session != null && targetPlayer.Messenger != null)
            {
                if (targetPlayer.Messenger.TryAddRequest(newRequest))
                {
                    await targetPlayer.Session.SendPacketAsync(new FriendRequestComposer(newRequest));
                }
            }
        }
    }
}
