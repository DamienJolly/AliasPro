using AliasPro.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Player.Models;
using AliasPro.Player.Packets.Outgoing;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    public class SecureLoginEvent : IAsyncPacket
    {
        public short Header { get; } = IncomingHeaders.SecureLoginMessageEvent;

        private readonly IPlayerController _playerController;

        public SecureLoginEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string ssoTicket = clientPacket.ReadString();
            IPlayer player = await _playerController.GetPlayerBySsoAsync(ssoTicket);
            if (player != null)
            {
                await session.WriteAndFlushAsync(new SecureLoginOKComposer());
            }
        }
    }
}
