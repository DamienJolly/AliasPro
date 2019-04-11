using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Player.Models;
using AliasPro.Messenger.Packets.Outgoing;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Player;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

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

        public async Task HandleAsync(
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
