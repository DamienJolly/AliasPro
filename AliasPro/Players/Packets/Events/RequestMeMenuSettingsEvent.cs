using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Players.Packets.Events
{
    public class RequestMeMenuSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestMeMenuSettingsMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new MeMenuSettingsComposer(session.Player.PlayerSettings));
        }
    }
}
