using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Models;
    using Packets.Outgoing;
    using Models.Messenger;

    public class FriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.FriendRequestMessageEvent;

        private readonly IPlayerController _playerController;

        public FriendRequestEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string username = clientPacket.ReadString();
            IPlayer targetPlayer = 
                await _playerController.GetPlayerByUsernameAsync(username);

            if (targetPlayer == null)
                return;

            if (session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                return;

            if (session.Player.Messenger.TryGetRequest(targetPlayer.Id, out IMessengerRequest request))
                return;

            IMessengerRequest newRequest = new MessengerRequest(session.Player);
            await _playerController.AddFriendRequestAsync(targetPlayer.Id, session.Player.Id);

            if (targetPlayer.Session != null && targetPlayer.Messenger != null)
            {
                targetPlayer.Messenger.AddRequest(newRequest);
                await targetPlayer.Session.SendPacketAsync(new FriendRequestComposer(newRequest));
            }
        }
    }
}
