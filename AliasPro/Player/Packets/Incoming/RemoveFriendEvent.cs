using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using Models;
    using Models.Messenger;

    public class RemoveFriendEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RemoveFriendMessageEvent;

        private readonly IPlayerController _playerController;

        public RemoveFriendEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int amount = clientPacket.ReadInt();
            for (int i = 0; i < amount; i++)
            {
                uint targetId = (uint)clientPacket.ReadInt();
                IPlayer targetPlayer = 
                    await _playerController.GetPlayerByIdAsync(targetId);

                if (targetPlayer == null)
                    return;

                if (session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                {
                    session.Player.Messenger.RemoveFriend(targetPlayer.Id);
                    await _playerController.RemoveFriendShipAsync(session.Player.Id, targetPlayer.Id);
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
