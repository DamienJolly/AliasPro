using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Messenger.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Messenger.Packets.Events
{
    public class AcceptFriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AcceptFriendRequestMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public AcceptFriendRequestEvent(IPlayerController playerController, IMessengerController messengerController)
        {
            _playerController = playerController;
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int amount = clientPacket.ReadInt();
            for (int i = 0; i < amount; i++)
            {
                uint targetId = (uint)clientPacket.ReadInt();

                if (!_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer))
                    continue;

                if (!session.Player.Messenger.TryGetRequest(targetPlayer.Id, out IMessengerRequest request))
                    continue;

                session.Player.Messenger.RemoveRequest(targetPlayer.Id);
                await _messengerController.RemoveRequestAsync(session.Player.Id, targetPlayer.Id);

                if (session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                    continue;

                IMessengerFriend friendOne = new MessengerFriend(targetPlayer);
                if (session.Player.Messenger.TryAddFriend(friendOne))
                {
                    await session.SendPacketAsync(new UpdateFriendComposer(friendOne));
                }

                if (targetPlayer.Session != null && targetPlayer.Messenger != null)
                {
                    IMessengerFriend friendTwo = new MessengerFriend(session.Player);
                    if (targetPlayer.Session.Player.Messenger.TryAddFriend(friendTwo))
                    {
                        await targetPlayer.Session.SendPacketAsync(new UpdateFriendComposer(friendTwo));
                    }
                }

                await _messengerController.AddFriendAsync(session.Player.Id, targetPlayer.Id);
            }
        }
    }
}
