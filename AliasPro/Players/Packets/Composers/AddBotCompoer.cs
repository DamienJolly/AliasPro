using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;

namespace AliasPro.Players.Packets.Composers
{
    public class AddBotCompoer : IMessageComposer
    {
		private readonly IPlayerBot _bot;

		public AddBotCompoer(IPlayerBot bot)
		{
			_bot = bot;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AddBotMessageCompoer);
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
