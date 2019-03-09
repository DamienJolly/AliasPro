using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using Models.Messenger;

    public class ChangeRelationEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ChangeRelationMessageEvent;

        private readonly IPlayerController _playerController;

        public ChangeRelationEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();
            if (!session.Player.Messenger.TryGetFriend((uint)playerId, out IMessengerFriend friend))
                return;

            int type = clientPacket.ReadInt();
            if (type < 0 || type > 3)
                return;

            friend.Relation = type;
            await _playerController.UpdateFriendRelationAsync(session.Player.Id, friend);
            await session.SendPacketAsync(new UpdateFriendComposer(friend));
        }
    }
}
