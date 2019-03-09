using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;

    public class RequestMeMenuSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestMeMenuSettingsMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new MeMenuSettingsComposer(session.Player.PlayerSettings));
        }
    }
}
