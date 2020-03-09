using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserCreditsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestUserCreditsMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            if (await session.Player.GetPlayerCurrency(0) != null)
            {
                await session.SendPacketAsync(new UserCreditsComposer((await session.Player.GetPlayerCurrency(-1)).Amount));
            }

            await session.SendPacketAsync(new UserCurrencyComposer(session.Player.Currency.Currencies));
        }
    }
}
