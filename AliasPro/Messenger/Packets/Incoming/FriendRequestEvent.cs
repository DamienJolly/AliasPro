using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Player.Models;
using AliasPro.Messenger.Models;
using AliasPro.Messenger.Packets.Outgoing;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Player;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

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

        public async Task HandleAsync(
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
