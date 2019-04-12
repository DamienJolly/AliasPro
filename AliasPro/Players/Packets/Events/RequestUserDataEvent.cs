using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserDataMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestUserDataEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new UserDataComposer(session.Player));
            await session.SendPacketAsync(new UserPerksComposer(session.Player));
        }
    }
}
