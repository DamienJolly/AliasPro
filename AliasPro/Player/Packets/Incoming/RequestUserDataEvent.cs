using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    public class RequestUserDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserDataMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestUserDataEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.WriteAndFlushAsync(new UserDataComposer(session.Player));
            await session.WriteAndFlushAsync(new UserPerksComposer(session.Player));
        }
    }
}
