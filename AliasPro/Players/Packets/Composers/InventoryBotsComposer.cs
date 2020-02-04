using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class InventoryBotsComposer : IMessageComposer
    {
        private readonly ICollection<IPlayerBot> _bots;

        public InventoryBotsComposer(ICollection<IPlayerBot> bots)
        {
			_bots = bots;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.InventoryBotsMessageComposer);
            message.WriteInt(_bots.Count);
            foreach (IPlayerBot bot in _bots)
            {
				message.WriteInt(bot.Id);
				message.WriteString(bot.Name);
				message.WriteString(bot.Motto);
				message.WriteString(bot.Gender == PlayerGender.MALE ? "m" : "f");
				message.WriteString(bot.Figure);
			};
			return message;
        }
    }
}
