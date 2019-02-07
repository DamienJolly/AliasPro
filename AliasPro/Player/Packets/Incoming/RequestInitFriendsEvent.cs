using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using Models.Messenger;

    public class RequestInitFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestInitFriendsMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestInitFriendsEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: add to cofig
            int maxFriends = 1000;

            if (session.Player.Messenger == null)
            {
                session.Player.Messenger = new MessengerHandler(
                    await _playerController.GetPlayerFriendsByIdAsync(session.Player.Id),
                    await _playerController.GetPlayerRequestsByIdAsync(session.Player.Id));
            }

            await session.SendPacketAsync(new MessengerInitComposer(maxFriends));
            await session.SendPacketAsync(new FriendsComposer(session.Player.Messenger.Friends));

            //todo: Offline messages
        }
    }
}
