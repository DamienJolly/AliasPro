using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class InventoryBotsComposer : IPacketComposer
    {
        private readonly ICollection<IPlayerBot> _bots;

        public InventoryBotsComposer(ICollection<IPlayerBot> bots)
        {
			_bots = bots;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.InventoryBotsMessageComposer);
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
