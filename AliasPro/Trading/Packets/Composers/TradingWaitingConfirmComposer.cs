using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradingWaitingConfirmComposer : IMessageComposer
    {
		public ServerMessage Compose() =>
			new ServerMessage(Outgoing.TradingWaitingConfirmMessageComposer);
    }
}
