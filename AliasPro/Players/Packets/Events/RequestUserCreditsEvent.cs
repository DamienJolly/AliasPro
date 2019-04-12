using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserCreditsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserCreditsMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new UserCreditsComposer(session.Player.Credits));
            await session.SendPacketAsync(new UserCurrencyComposer(session.Player.Currency.Currencies));
        }
    }
}
