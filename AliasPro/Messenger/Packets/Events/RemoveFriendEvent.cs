using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Messenger.Packets.Events
{
    public class RemoveFriendEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RemoveFriendMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IMessengerController _messengerController;

        public RemoveFriendEvent(IPlayerController playerController, IMessengerController messengerController)
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
                    return;

                if (session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                {
                    session.Player.Messenger.RemoveFriend(targetPlayer.Id);
                    await _messengerController.RemoveFriendAsync(session.Player.Id, targetPlayer.Id);
                    await session.SendPacketAsync(new UpdateFriendComposer(targetPlayer.Id));

                    if (targetPlayer.Session != null && targetPlayer.Messenger != null)
                    {
                        targetPlayer.Messenger.RemoveFriend(session.Player.Id);
                        await targetPlayer.Session.SendPacketAsync(new UpdateFriendComposer(session.Player.Id));
                    }
                }
            }
        }
    }
}
