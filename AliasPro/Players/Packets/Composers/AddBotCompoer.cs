using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;

namespace AliasPro.Players.Packets.Composers
{
    public class AddBotCompoer : IPacketComposer
    {
		private readonly IPlayerBot _bot;

		public AddBotCompoer(IPlayerBot bot)
		{
			_bot = bot;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddBotMessageCompoer);
			message.WriteInt(_bot.Id);
			message.WriteString(_bot.Name);
			message.WriteString(_bot.Motto);
			message.WriteString(_bot.Gender == PlayerGender.MALE ? "m" : "f");
			message.WriteString(_bot.Figure);
			message.WriteBoolean(true); //?
			return message;
        }
    }
}
