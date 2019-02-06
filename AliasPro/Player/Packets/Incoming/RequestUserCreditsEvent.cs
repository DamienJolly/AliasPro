using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    public class RequestUserCreditsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserCreditsMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new UserCreditsComposer(session.Player.Credits));
            await session.SendPacketAsync(new UserCurrencyComposer(session.Player.Currency.Currencies));
        }
    }
}
