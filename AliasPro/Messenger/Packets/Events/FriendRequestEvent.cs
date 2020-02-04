using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Models;
using AliasPro.Messenger.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class FriendRequestEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.FriendRequestMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public FriendRequestEvent(IPlayerController playerController, IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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
