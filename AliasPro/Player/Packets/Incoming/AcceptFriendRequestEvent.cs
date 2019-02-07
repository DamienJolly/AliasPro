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

    public class AcceptFriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AcceptFriendRequestMessageEvent;

        private readonly IPlayerController _playerController;

        public AcceptFriendRequestEvent(IPlayerController playerController)
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

                if (session.Player.Messenger.TryGetRequest(targetPlayer.Id, out IMessengerRequest request))
                {
                    session.Player.Messenger.RemoveRequest(targetPlayer.Id);
                    await _playerController.RemoveFriendRequestAsync(session.Player.Id, targetPlayer.Id);

                    if (!session.Player.Messenger.TryGetFriend(targetPlayer.Id, out IMessengerFriend friend))
                    {
                        IMessengerFriend friendOne = new MessengerFriend(targetPlayer);
                        session.Player.Messenger.AddFriend(friendOne);
                        await _playerController.AddFriendShipAsync(session.Player.Id, targetPlayer.Id);
                        await session.SendPacketAsync(new UpdateFriendComposer(friendOne));

                        if (targetPlayer.Session != null && targetPlayer.Messenger != null)
                        {
                            IMessengerFriend friendTwo = new MessengerFriend(session.Player);
                            targetPlayer.Session.Player.Messenger.AddFriend(friendOne);
                            await session.SendPacketAsync(new UpdateFriendComposer(friendTwo));
                        }
                    }
                }
            }
        }
    }
}
